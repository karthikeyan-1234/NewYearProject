using Purchases.Domain.Entities;
using Purchases.Services.DTOs;

namespace Purchases.Services
{
    public interface IPurchaseService
    {
        #region Master Purchase CUD Operations

        Task<Purchase> AddPurchaseAsync(Purchase Purchase);
        Task<Purchase> UpdatePurchaseAsync(Purchase Purchase);
        Task<Purchase> DeletePurchaseAsync(Purchase Purchase);

        #endregion

        #region Single Purchase Detail CUD Operations

        Task<PurchaseDetail> AddPurchaseDetailAsync(PurchaseDetail PurchaseDetail);
        Task<PurchaseDetail> UpdatePurchaseDetailAsync(PurchaseDetail PurchaseDetail);
        Task<PurchaseDetail> DeletePurchaseDetailAsync(PurchaseDetail PurchaseDetail);

        #endregion

        #region Multiple Purchase Detail CUD Operations


        Task AddPurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails);
        Task UpdatePurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails);
        Task DeletePurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails);

        #endregion

        #region Read Operations
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
        Task<Purchase> GetPurchaseByIdAsync(int id);
        Task<IEnumerable<PurchaseDetail>> GetAllPurchaseDetailsAsync();
        Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int id);
        Task<IEnumerable<PurchaseDetail>> GetAllPurchaseDetailsByPurchaseIdAsync(int purchaseId);
        PurchaseResponse GetPurchaseDetailsForPurchase(int purchaseId);
        #endregion
    }
}