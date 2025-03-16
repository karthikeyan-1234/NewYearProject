using Sales.Domain;
using Sales.Domain.Contracts;

using Sales.Domain.Entities;
using Sales.Services.DTOs;

using System.Text.Json;


namespace Sales.Services
{
    public class SalesService : ISalesService
    {
        IGenericRepository<Sale> _saleRepo;
        IGenericRepository<SaleDetail> _saleDetailRepo;
        IMessageService messageService;
        ICachingService cachingService;

        public SalesService(IGenericRepository<Sale> saleRepo, IGenericRepository<SaleDetail> saleDetailRepo, IMessageService messageService, ICachingService cachingService)
        {
            _saleRepo = saleRepo;
            _saleDetailRepo = saleDetailRepo;
            this.messageService = messageService;
            this.cachingService = cachingService;
        }

        public async Task<SaleDetail> AddSaleDetailAsync(SaleDetail saleDetail)
        {
            var newSaleDetail = await _saleDetailRepo.AddAsync(saleDetail);
            await _saleDetailRepo.SaveChangesAsync();
            return newSaleDetail;
        }

        public async Task<SaleDetail> DeleteSaleDetailAsync(SaleDetail saleDetail)
        {
            var deletedSaleDetail = _saleDetailRepo.DeleteAsync(saleDetail);
            await _saleDetailRepo.SaveChangesAsync();
            return deletedSaleDetail;
        }

        public async Task<SaleDetail> UpdateSaleDetailAsync(SaleDetail saleDetail)
        {
            //Get the current sale detail
            var currentSaleDetail = await _saleDetailRepo.GetByIdAsync(saleDetail.Id);

            if (currentSaleDetail == null) //Add the sale detail if it does not exist
                return await AddSaleDetailAsync(saleDetail);

            var updatedSaleDetail = await _saleDetailRepo.UpdateAsync(saleDetail);
            await _saleDetailRepo.SaveChangesAsync();

            var quantity = saleDetail.Quantity - currentSaleDetail!.Quantity;
            return updatedSaleDetail;
        }

        public async Task<IEnumerable<SaleDetail>> UpdateSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails)
        {
            var updatedSaleDetails = await _saleDetailRepo.UpdateRangeAsync(saleDetails);
            await _saleDetailRepo.SaveChangesAsync();

            await messageService.PublishMessage(updatedSaleDetails, "SaleDetails.Updated");
            return updatedSaleDetails;
        }

        public async Task AddSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails)
        {
            var addedSaleDetails = await _saleDetailRepo.AddRangeAsync(saleDetails);
            await _saleDetailRepo.SaveChangesAsync();

            await messageService.PublishMessage(addedSaleDetails, "SaleDetails.Added");
        }

        public async Task<IEnumerable<SaleDetail>> DeleteSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails)
        {
            var deletedSaleDetails = _saleDetailRepo.DeleteRange(saleDetails);
            await _saleDetailRepo.SaveChangesAsync();

            await messageService.PublishMessage(deletedSaleDetails, "SaleDetails.Deleted");
            return deletedSaleDetails;
        }


        #region Others

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            var newSale = await _saleRepo.AddAsync(sale);
            await _saleRepo.SaveChangesAsync();
            return newSale;
        }

        public async Task<Sale> DeleteSaleAsync(Sale sale)
        {
            var deletedSale = _saleRepo.DeleteAsync(sale);
            await _saleRepo.SaveChangesAsync();
            await messageService.PublishMessage(sale, "Sale.Deleted");
            return deletedSale;
        }

        public async Task<IEnumerable<SaleDetail>> GetAllSaleDetailsAsync()
        {
            return await _saleDetailRepo.GetAllAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepo.GetAllAsync();
        }

        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            var sale = await _saleRepo.GetByIdAsync(id);
            return sale;
        }

        public async Task<SaleDetail?> GetSaleDetailByIdAsync(int id)
        {
            return await _saleDetailRepo.GetByIdAsync(id);
        }

        public SaleResponse GetSaleDetailsForSale(int saleId)
        {
            var sale =  _saleRepo.Find(s => s.Id == saleId).FirstOrDefault();
            var saleDetails = _saleDetailRepo.Find(sd => sd.SaleId == sale!.Id).ToArray();

            return new SaleResponse
            {
                Result = sale,
                SaleDetails = saleDetails
            };

        }

        public async Task<Sale> UpdateSaleAsync(Sale sale)
        {
             var updatedSale = await _saleRepo.UpdateAsync(sale);
            await _saleRepo.SaveChangesAsync();
            await messageService.PublishMessage(sale, "Sale.Updated");
            return updatedSale;
        }

        #endregion
    }
}
