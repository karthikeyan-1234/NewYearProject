using Reporting.SalesWebAPI.Models;

namespace Reporting.SalesWebAPI.Services
{
    public interface IPurchaseReportingService
    {
        Task<IEnumerable<PurchaseDetailInfo>> GetPurchaseInfo(int purchaseId);
        Task<IEnumerable<PurchaseInfo>> GetAllPurchasesInfo();
    }
}
