using Domain.Contracts;

using Inventory.Domain;
using Inventory.Domain.Contracts;
using Inventory.Domain.Entities;
using Inventory.Services.Contracts;

namespace Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<InventoryEntry> _inventoryRepo;

        public InventoryService(IGenericRepository<InventoryEntry> inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        #region Others

        public async Task AddInventoryEntry(InventoryEntry entry)
        {
            var lastEntry = await _inventoryRepo.LastAsync();

            if (lastEntry == null)
                entry.Id = 1;
            else
            entry.Id = (await _inventoryRepo.LastAsync())!.Id + 1;

            await _inventoryRepo.AddAsync(entry);
            await _inventoryRepo.SaveChangesAsync();
        }

        public async Task UpdateInventoryEntry(InventoryEntry entry)
        {
            await _inventoryRepo.UpdateAsync(entry);
            await _inventoryRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryEntry>> GetAllInventoryEntries()
        {
            return await _inventoryRepo.GetAllAsync();
        }

        public async Task<InventoryEntry> GetInventoryEntryById(int id)
        {
            return await _inventoryRepo.GetByIdAsync(id);
        }

        public async Task RemoveInventoryEntry(int id)
        {
            var entry = await _inventoryRepo.GetByIdAsync(id);
            _inventoryRepo.DeleteAsync(entry);
            await _inventoryRepo.SaveChangesAsync();
        }

        #endregion

        #region Single Purchase Detail CRUD Operations

        public async Task AddPurchaseDetail(PurchaseDetail purchaseDetail)
        {
            InventoryEntry entry = new()
            {
                ProductId = purchaseDetail.ProductId,
                OrderItemNo = purchaseDetail.Id,
                OrderRefNo = purchaseDetail.PurchaseId.ToString(),
                Quantity = purchaseDetail.Quantity,
                OrderType = OrderType.Purchase,
            };

            await _inventoryRepo.AddAsync(entry);
            await _inventoryRepo.SaveChangesAsync();
        }

        public async Task UpdatePurchaseDetail(PurchaseDetail purchaseDetail)
        {
            InventoryEntry entry = _inventoryRepo.Find(x => x.OrderRefNo == purchaseDetail.PurchaseId.ToString() && x.OrderItemNo == purchaseDetail.Id).FirstOrDefault()!;

            if (entry != null)
            {
                entry.ProductId = purchaseDetail.ProductId;
                entry.Quantity = purchaseDetail.Quantity;
                await _inventoryRepo.UpdateAsync(entry);
                await _inventoryRepo.SaveChangesAsync();
            }
        }

        public async Task RemovePurchaseDetail(int id)
        {
            //Find the inventory entry by OrderItemNo

           var inventory = (_inventoryRepo.Find(x => x.OrderItemNo == id && x.OrderType == OrderType.Purchase)).FirstOrDefault();
            if (inventory != null)
            {
                _inventoryRepo.DeleteAsync(inventory);
                await _inventoryRepo.SaveChangesAsync();
            }
        }

        #endregion

        #region Multiple Purchase Detail CRUD Operations
        public Task AddPurchaseDetails(IEnumerable<PurchaseDetail> purchaseDetails)
        {
            var inventories = purchaseDetails.Select(purchaseDetail => new InventoryEntry
            {
                ProductId = purchaseDetail.ProductId,
                OrderItemNo = purchaseDetail.Id,
                OrderRefNo = purchaseDetail.PurchaseId.ToString(),
                Quantity = purchaseDetail.Quantity,
                OrderType = OrderType.Purchase,
            }).ToList();

            _inventoryRepo.AddRangeAsync(inventories);
            return _inventoryRepo.SaveChangesAsync();
        }

        public Task UpdatePurchaseDetails(IEnumerable<PurchaseDetail> purchaseDetails)
        {
            var inventories = purchaseDetails.Select(purchaseDetail => new InventoryEntry
            {
                ProductId = purchaseDetail.ProductId,
                OrderItemNo = purchaseDetail.Id,
                OrderRefNo = purchaseDetail.PurchaseId.ToString(),
                Quantity = purchaseDetail.Quantity,
                OrderType = OrderType.Purchase,
            });
            _inventoryRepo.UpdateRangeAsync(inventories);
            return _inventoryRepo.SaveChangesAsync();
        }

        public async Task RemovePurchaseDetailsAsync(IEnumerable<PurchaseDetail> purchaseDetails)
        {
            var inventories = (from inv in _inventoryRepo.Table.Where(x => x.OrderType == OrderType.Purchase && purchaseDetails.Any(s => s.PurchaseId.ToString() == x.OrderRefNo && s.Id == x.OrderItemNo))
                               select inv).ToList();
            _inventoryRepo.DeleteRange(inventories);
            await _inventoryRepo.SaveChangesAsync();
        }
        #endregion

        #region Multiple Sales Detail CRUD Operations
        public Task AddSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails)
        {
            var inventories = salesDetails.Select(saleDetail => new InventoryEntry
            {
                ProductId = saleDetail.ProductId,
                OrderItemNo = saleDetail.Id,
                OrderRefNo = saleDetail.SaleId.ToString(),
                Quantity = saleDetail.Quantity * -1,
                OrderType = OrderType.Sale,
            }).ToList();
            _inventoryRepo.AddRangeAsync(inventories);
            return _inventoryRepo.SaveChangesAsync();
        }

        public Task UpdateSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails)
        {
            var inventories = (from inv in _inventoryRepo.Table.Where(x => x.OrderType == OrderType.Sale && salesDetails.Any(s => s.SaleId.ToString() == x.OrderRefNo && s.Id == x.OrderItemNo) )
                              select inv).ToList();

            _inventoryRepo.UpdateRangeAsync(inventories);
            return _inventoryRepo.SaveChangesAsync();
        }

        public async Task RemoveSalesDetailsAsync(IEnumerable<SaleDetail> salesDetails)
        {
            var inventories = (from inv in _inventoryRepo.Table.Where(x => x.OrderType == OrderType.Sale && salesDetails.Any(s => s.SaleId.ToString() == x.OrderRefNo && s.Id == x.OrderItemNo))
                               select inv).ToList();
            _inventoryRepo.DeleteRange(inventories);
            await _inventoryRepo.SaveChangesAsync();
        }
        #endregion
    }
}
