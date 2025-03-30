using Reporting.SalesWebAPI.Models;

using Sales.Domain.Entities;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sales.Reporting.Services
{
    public class SalesReportingService : ISalesReportingService
    {
        IHttpClientFactory httpClientFactory;
        HttpClient saleClient, masterClient, tokenClient;
        private List<KeyValuePair<string, string>> requestData;

        public SalesReportingService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

            this.saleClient = httpClientFactory.CreateClient();
            this.masterClient = httpClientFactory.CreateClient();

            var clientId = "api-app";
            var grantType = "password";
            var userName = "mail2karthikkn@gmail.com";
            var password = "Arjun123*";
            var clientSecret = "0NhsqAtEogqzbrIgVSXbFytANfvm0UW4";
            var scope = "openid profile email";
            var resource = "masters";

            this.tokenClient = httpClientFactory.CreateClient();

            //set header --header 'Content-Type: application/x-www-form-urlencoded'
            tokenClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

            requestData = new List<KeyValuePair<string, string>>
            {
                new("client_id", clientId),
                new("client_secret", clientSecret),
                new("grant_type", grantType),
                new("username", userName),
                new("password", password),
                new("scope", scope),
                new("resource", resource),
            };



            saleClient.BaseAddress = new Uri("https://localhost:7029/api/Sales");
            masterClient.BaseAddress = new Uri("https://localhost:7271/api/Masters");
            
        }

        public class SaleResponse
        {
            public Sale? Result { get; set; }
            public SaleDetail[]? SaleDetails { get; set; }
        }

        public class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string? AccessToken { get; set; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("token_type")]
            public string? TokenType { get; set; }

            // You might have other properties in the response, like "refresh_token"
            // [JsonPropertyName("refresh_token")]
            // public string? RefreshToken { get; set; }
        }

        public async Task<IEnumerable<SaleDetailInfo>> GetSaleInfo(int saleId)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


            var saleUrl = saleClient.BaseAddress + $"/GetSaleDetailsForSale/{saleId}";
            var saleDetailResponse = await saleClient.GetAsync(saleUrl);
            var result = await saleDetailResponse.Content.ReadAsStringAsync();
            var sale = JsonSerializer.Deserialize<SaleResponse>(result, options);
            var saleDetails = sale!.SaleDetails;

            var productsInSale = saleDetails!.Select(sd => sd.ProductId).ToList();

            var productUrl = masterClient.BaseAddress + $"/GetProductsByIds";



            var requestContent = new FormUrlEncodedContent(requestData);
            var response = await tokenClient.PostAsync("http://localhost:8080/realms/master/protocol/openid-connect/token", requestContent);

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(await response.Content.ReadAsStringAsync());
            var accessToken = tokenResponse!.AccessToken;

            // Set the Authorization header with the Bearer token
            masterClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var productResponse = await masterClient.PostAsync(productUrl, new StringContent(JsonSerializer.Serialize(productsInSale), Encoding.UTF8, "application/json"));
            var productsList = await productResponse.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(productsList, options);

            var finalResult = (from sd in saleDetails!
                              join p in products! on sd.ProductId equals p.Id into productGroup
                              from p in productGroup.DefaultIfEmpty()
                              select new SaleDetailInfo
                              {
                                  Id = sd.Id,
                                  SaleId = sd.SaleId,
                                  Price = (decimal)sd.Price,
                                  ProductId = sd.ProductId,
                                  Quantity = (int)sd.Quantity,
                                  ProductName = p.Name
                              }).ToList();

            return finalResult;
        }

        public async Task<IEnumerable<SaleInfo>> GetAllSalesInfo()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var saleUrl = saleClient.BaseAddress + $"/GetAllSalesAsync";
            var saleResponse = await saleClient.GetAsync(saleUrl);
            var result = await saleResponse.Content.ReadAsStringAsync();
            var sales = JsonSerializer.Deserialize<IEnumerable<SaleInfo>>(result, options)!;

            var customerIds = sales.Select(s => s.CustomerId).ToList();


            var requestContent = new FormUrlEncodedContent(requestData);
            var response = await tokenClient.PostAsync("http://localhost:8080/realms/master/protocol/openid-connect/token", requestContent);

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(await response.Content.ReadAsStringAsync());
            var accessToken = tokenResponse!.AccessToken;

            // Set the Authorization header with the Bearer token
            masterClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var customerUrl = masterClient.BaseAddress + $"/GetCustomersByIds";
            var customerResponse = await masterClient.PostAsync(customerUrl, new StringContent(JsonSerializer.Serialize(customerIds), Encoding.UTF8, "application/json"));

            if(!customerResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error while fetching customers");
            }
            var customersList = await customerResponse.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(customersList, options);

            var finalResult = (from s in sales
                               join c in customers! on s.CustomerId equals c.Id into customerGroup
                               from c in customerGroup.DefaultIfEmpty()
                               select new SaleInfo
                               {
                                   Id = s.Id,
                                   CustomerId = s.CustomerId,
                                   SaleDate = s.SaleDate,
                                   CustomerName = c.Name
                               }).ToList();
            return finalResult;
        }
    }
}
