using Inventory.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Contracts
{
    public interface IInventoryService
    {
        Task AddInventoryEntry(InventoryEntry entry);
        Task<IEnumerable<InventoryEntry>> GetAllInventoryEntries();
        Task<InventoryEntry> GetInventoryEntryById(int id);
        Task RemoveInventoryEntry(int id);
        Task UpdateInventoryEntry(InventoryEntry entry);


        #region Single Purchase Detail CRUD Operations
        Task AddPurchaseDetail(PurchaseDetail purchaseDetail);
        Task UpdatePurchaseDetail(PurchaseDetail purchaseDetail);
        Task RemovePurchaseDetail(int id);
        #endregion


        #region Multiple Purchase Detail CRUD Operations
        Task AddPurchaseDetails(IEnumerable<PurchaseDetail> purchaseDetails);
        Task UpdatePurchaseDetails(IEnumerable<PurchaseDetail> purchaseDetails);
        Task RemovePurchaseDetailsAsync(IEnumerable<PurchaseDetail> purchaseDetails);
        #endregion


        #region Multiple Sales Detail CRUD Operations
        Task AddSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails);
        Task UpdateSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails);
        Task RemoveSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails);
        #endregion
    }
}
