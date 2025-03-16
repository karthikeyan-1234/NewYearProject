using Purchases.Domain.Entities;

using Reporting.SalesWebAPI.Models;

using Sales.Domain.Entities;

using System.Text;
using System.Text.Json;

namespace Reporting.SalesWebAPI.Services
{
    public class PurchaseReportingService : IPurchaseReportingService
    {
        HttpClient purchaseClient, masterClient;

        public PurchaseReportingService(IHttpClientFactory httpClientFactory)
        {
            purchaseClient = httpClientFactory.CreateClient("PurchaseClient");
            masterClient = httpClientFactory.CreateClient("MasterClient");

            purchaseClient.BaseAddress = new Uri("https://localhost:7286/api/Purchase");
            masterClient.BaseAddress = new Uri("https://localhost:7271/api/Masters");
        }

        public class PurchaseResponse
        {
            public Purchase? Result { get; set; }
            public PurchaseDetail[]? PurchaseDetails { get; set; }
        }

        public async Task<IEnumerable<PurchaseDetailInfo>> GetPurchaseInfo(int purchaseId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


                var Url = purchaseClient.BaseAddress + $"/GetPurchaseDetailsForPurchase/{purchaseId}";
                var purchaseDetailResponse = await purchaseClient.GetAsync(Url);
                var result = await purchaseDetailResponse.Content.ReadAsStringAsync();
                var purchase = JsonSerializer.Deserialize<PurchaseResponse>(result, options);
                var purchaseDetails = purchase!.PurchaseDetails;

                var productsInPurchase = purchaseDetails!.Select(sd => sd.ProductId).ToList();

                var productUrl = masterClient.BaseAddress + $"/GetProductsByIds";
                var productResponse = await masterClient.PostAsync(productUrl, new StringContent(JsonSerializer.Serialize(productsInPurchase), Encoding.UTF8, "application/json"));
                var productsList = await productResponse.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(productsList, options);

                var finalResult = (from pd in purchaseDetails!
                                   join p in products! on pd.ProductId equals p.Id into productGroup
                                   from p in productGroup.DefaultIfEmpty()
                                   select new PurchaseDetailInfo
                                   {
                                       Id = pd.Id,
                                       PurchaseId = pd.PurchaseId,
                                       Price = (decimal)pd.Price,
                                       ProductId = pd.ProductId,
                                       Quantity = (int)pd.Quantity,
                                       ProductName = p.Name
                                   }).ToList();

                return finalResult;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<IEnumerable<PurchaseInfo>> GetAllPurchasesInfo()
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var purchaseUrl = purchaseClient.BaseAddress + $"/GetAllPurchasesAsync";
                var saleResponse = await purchaseClient.GetAsync(purchaseUrl);
                var result = await saleResponse.Content.ReadAsStringAsync();
                var purchases = JsonSerializer.Deserialize<IEnumerable<PurchaseInfo>>(result, options)!;

                var vendorIds = purchases.Select(s => s.VendorId).ToList();

                //The below should be replaced by Vendor API call, when it is available

                var customerUrl = masterClient.BaseAddress + $"/GetCustomersByIds";
                var customerResponse = await masterClient.PostAsync(customerUrl, new StringContent(JsonSerializer.Serialize(vendorIds), Encoding.UTF8, "application/json"));
                var customersList = await customerResponse.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(customersList, options);


                //Final result

                var finalResult = (from s in purchases
                                   join c in customers! on s.VendorId equals c.Id into customerGroup
                                   from c in customerGroup.DefaultIfEmpty()
                                   select new PurchaseInfo
                                   {
                                       Id = s.Id,
                                       VendorId = s.VendorId,
                                       PurchaseDate = s.PurchaseDate,
                                       VendorName = c.Name
                                   }).ToList();
                return finalResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
