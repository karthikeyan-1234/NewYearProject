using Purchases.Services;

using Purchases.Domain;
using Purchases.Domain.Contracts;

using Purchases.Domain.Entities;

using System.Text.Json;
using Domain.Contracts;
using Purchases.Services.DTOs;

namespace Purchases.Services
{
    public class PurchaseService : IPurchaseService
    {
        IGenericRepository<Purchase> _PurchaseRepo;
        IGenericRepository<PurchaseDetail> _PurchaseDetailRepo;
        IMessageService messageService;
        ICachingService cachingService;

        public PurchaseService(IGenericRepository<Purchase> PurchaseRepo, IGenericRepository<PurchaseDetail> PurchaseDetailRepo, IMessageService messageService, ICachingService cachingService)
        {
            _PurchaseRepo = PurchaseRepo;
            _PurchaseDetailRepo = PurchaseDetailRepo;
            this.messageService = messageService;
            this.cachingService = cachingService;
        }

        #region Single Purchase Detail CRUD Operations

        public async Task<PurchaseDetail> AddPurchaseDetailAsync(PurchaseDetail PurchaseDetail)
        {
            var newPurchaseDetail = await _PurchaseDetailRepo.AddAsync(PurchaseDetail);
            await _PurchaseDetailRepo.SaveChangesAsync();
            await messageService.PublishMessage(newPurchaseDetail, "PurchaseDetail.Added");
            return newPurchaseDetail;
        }


        public async Task<PurchaseDetail> UpdatePurchaseDetailAsync(PurchaseDetail PurchaseDetail)
        {
            //Get the current Purchase detail
            var currentPurchaseDetail = await _PurchaseDetailRepo.GetByIdAsync(PurchaseDetail.Id);

            if (currentPurchaseDetail == null)
            {
                var newPurchaseDetail = await _PurchaseDetailRepo.AddAsync(PurchaseDetail);
                await _PurchaseDetailRepo.SaveChangesAsync();
                await messageService.PublishMessage(newPurchaseDetail, "PurchaseDetail.Added");
                return newPurchaseDetail;
            }
            else
            {
                try
                {
                    await _PurchaseDetailRepo.UpdateEntry(currentPurchaseDetail,PurchaseDetail);
                    await _PurchaseDetailRepo.SaveChangesAsync();
                    var quantity = PurchaseDetail.Quantity - currentPurchaseDetail.Quantity;
                    await messageService.PublishMessage(PurchaseDetail, "PurchaseDetail.Updated");
                    return currentPurchaseDetail;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<PurchaseDetail> DeletePurchaseDetailAsync(PurchaseDetail PurchaseDetail)
        {
             _PurchaseDetailRepo.DeleteAsync(PurchaseDetail);
            await _PurchaseDetailRepo.SaveChangesAsync();


            await messageService.PublishMessage(PurchaseDetail, "PurchaseDetail.Deleted");
            return PurchaseDetail;
        }

        #endregion

        #region Multiple Purchase Details CRUD Operations

        //Add Purchase details

        public async Task AddPurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails)
        {
            await _PurchaseDetailRepo.AddRangeAsync(PurchaseDetails);
            await _PurchaseDetailRepo.SaveChangesAsync();
            await messageService.PublishMessage(PurchaseDetails, "PurchaseDetails.Added");
        }

        //Update Purchase details

        public async Task UpdatePurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails)
        {
            await _PurchaseDetailRepo.UpdateRangeAsync(PurchaseDetails);
            await _PurchaseDetailRepo.SaveChangesAsync();
            await messageService.PublishMessage(PurchaseDetails, "PurchaseDetails.Updated");
        }

        //Delete Purchase details

        public async Task DeletePurchaseDetailsAsync(IEnumerable<PurchaseDetail> PurchaseDetails)
        {
            _PurchaseDetailRepo.DeleteRange(PurchaseDetails);
            await _PurchaseDetailRepo.SaveChangesAsync();
            await messageService.PublishMessage(PurchaseDetails, "PurchaseDetails.Deleted");
        }

        #endregion

        #region Master Purchase CRUD Operations

        public async Task<Purchase> AddPurchaseAsync(Purchase Purchase)
        {
            var newPurchase = await _PurchaseRepo.AddAsync(Purchase);
            await _PurchaseRepo.SaveChangesAsync();
            return newPurchase;
        }

        public async Task<Purchase> DeletePurchaseAsync(Purchase Purchase)
        {
            //Delete all the Purchase details

            var PurchaseDetails =  _PurchaseDetailRepo.Find(p => p.PurchaseId == Purchase.Id);
            _PurchaseRepo.DeleteAsync(Purchase);
            await _PurchaseRepo.SaveChangesAsync();
            return Purchase;
        }

        public async Task<Purchase> UpdatePurchaseAsync(Purchase Purchase)
        {
            try
            {
                var updatedPurchase = await _PurchaseRepo.UpdateAsync(Purchase);
                await _PurchaseRepo.SaveChangesAsync();
                //await messageService.PublishMessage(Purchase, "Purchase.Updated");
                return updatedPurchase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Read Operations

        public async Task<IEnumerable<PurchaseDetail>> GetAllPurchaseDetailsAsync()
        {
            return await _PurchaseDetailRepo.GetAllAsync();
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
        {
            var purchases = await _PurchaseRepo.GetAllAsync();
            return purchases;
        }

        public async Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            return await _PurchaseRepo.GetByIdAsync(id);
        }

        public async Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int id)
        {
            return await _PurchaseDetailRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PurchaseDetail>> GetAllPurchaseDetailsByPurchaseIdAsync(int purchaseId)
        {
            return await Task.FromResult(_PurchaseDetailRepo.Find(p => p.PurchaseId == purchaseId).ToList());
        }



        public PurchaseResponse GetPurchaseDetailsForPurchase(int purchaseId)
        {
            var purchase = _PurchaseRepo.Find(s => s.Id == purchaseId).FirstOrDefault();
            var purchaseDetails = _PurchaseDetailRepo.Find(sd => sd.PurchaseId == purchase!.Id).ToArray();

            return new PurchaseResponse
            {
                Result = purchase,
                PurchaseDetails = purchaseDetails
            };

        }



        #endregion
    }
}
