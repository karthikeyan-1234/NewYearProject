using Sales.Domain.Entities;

namespace Sales.Services
{
    public class SaleResponse
    {
        public Sale? Result { get; set; }
        public SaleDetail[]? SaleDetails { get; set; }
    }

    public interface ISalesService
    {
        Task<Sale> AddSaleAsync(Sale sale);
        Task<Sale> UpdateSaleAsync(Sale sale);
        Task<Sale> DeleteSaleAsync(Sale sale);

        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale?> GetSaleByIdAsync(int id);

        Task<SaleDetail> AddSaleDetailAsync(SaleDetail saleDetail);
        Task AddSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails);

        Task<SaleDetail> UpdateSaleDetailAsync(SaleDetail saleDetail);
        Task<IEnumerable<SaleDetail>> UpdateSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails);

        Task<SaleDetail> DeleteSaleDetailAsync(SaleDetail saleDetail);
        Task<IEnumerable<SaleDetail>> DeleteSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails);

        Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync();
        Task<SaleDetail?> GetSaleDetailByIdAsync(int id);
        SaleResponse GetSaleDetailsForSale(int saleId);
    }
}