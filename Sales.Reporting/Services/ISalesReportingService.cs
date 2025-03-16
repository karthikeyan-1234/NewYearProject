
using Reporting.SalesWebAPI.Models;

namespace Sales.Reporting.Services
{
    public interface ISalesReportingService
    {
        Task<IEnumerable<SaleDetailInfo>> GetSaleInfo(int saleId);
        Task<IEnumerable<SaleInfo>> GetAllSalesInfo();
    }
}