using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sales.Domain.Entities;
using Sales.Services;

namespace Sales.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        #region Others

        // POST: Add new Sale
        [HttpPost("AddSaleAsync")]
        public async Task<IActionResult> AddSaleAsync([FromBody] Sale sale)
        {
            await _salesService.AddSaleAsync(sale);
            return CreatedAtAction("AddSale", sale);
        }

        // GET: Get all Sales
        [HttpGet("GetAllSalesAsync")]
        public async Task<IActionResult> GetAllSalesAsync()
        {
            var sales = await _salesService.GetAllSalesAsync();
            return Ok(sales);
        }

        // GET: Get Sale by Id
        [HttpGet("GetSaleByIdAsync/{id}")]
        public async Task<IActionResult> GetSaleByIdAsync(int id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            return Ok(sale);
        }

        // PUT: Update Sale
        [HttpPut("UpdateSaleAsync")]
        public async Task<IActionResult> UpdateSaleAsync([FromBody] Sale sale)
        {
            await _salesService.UpdateSaleAsync(sale);
            return NoContent();
        }

        // DELETE: Delete Sale
        [HttpDelete("DeleteSaleAsync")]
        public async Task<IActionResult> DeleteSaleAsync([FromBody] Sale sale)
        {
            await _salesService.DeleteSaleAsync(sale);
            return NoContent();
        }

        // DELETE: Delete Sale with id
        [HttpDelete("DeleteSaleByIdAsync/{id}")]
        public async Task<IActionResult> DeleteSaleByIdAsync(int id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            await _salesService.DeleteSaleAsync(sale);
            return NoContent();
        }

        // POST: Add new SaleDetail
        [HttpPost("AddSaleDetailAsync")]
        public async Task<IActionResult> AddSaleDetailAsync([FromBody] SaleDetail saleDetail)
        {
            await _salesService.AddSaleDetailAsync(saleDetail);
            return CreatedAtAction("AddSaleDetail", saleDetail);
        }

        // GET: Get all SaleDetails
        [HttpGet("GetAllSaleDetailsAsync")]
        public async Task<IActionResult> GetAllSaleDetailsAsync()
        {
            var saleDetails = await _salesService.GetAllSaleDetailsAsync();
            return Ok(saleDetails);
        }

        // GET: Get SaleDetail by Id
        [HttpGet("GetSaleDetailByIdAsync/{id}")]
        public async Task<IActionResult> GetSaleDetailByIdAsync(int id)
        {
            var saleDetail = await _salesService.GetSaleByIdAsync(id);
            return Ok(saleDetail);
        }

        #endregion

        // GET: Get SaleDetail for Sale
        [HttpGet("GetSaleDetailsForSale/{saleId}")]
        public IActionResult GetSaleDetailsForSale(int saleId)
        {
            var saleDetails = _salesService.GetSaleDetailsForSale(saleId);
            return Ok(saleDetails);
        }

        // POST: Add new SaleDetail
        [HttpPost("AddSaleDetailsAsync")]
        public async Task<IActionResult> AddSaleDetailsAsync([FromBody] IEnumerable<SaleDetail> saleDetails)
        {
            await _salesService.AddSaleDetailsAsync(saleDetails);
            return Ok(saleDetails);
        }

        // PUT: Update SaleDetail
        [HttpPut("UpdateSaleDetailsAsync")]
        public async Task<IActionResult> UpdateSaleDetailsAsync([FromBody] IEnumerable<SaleDetail> saleDetails)
        {
            var updatedSaleDetails = await _salesService.UpdateSaleDetailsAsync(saleDetails);
            return Ok(updatedSaleDetails);
        }

        // PUT: Update SaleDetail
        [HttpPut("UpdateSaleDetailAsync")]
        public async Task<IActionResult> UpdateSaleDetailAsync([FromBody] SaleDetail saleDetail)
        {
            var updatedSaleDetails = await _salesService.UpdateSaleDetailAsync(saleDetail);
            return Ok(updatedSaleDetails);
        }

        //DELETE: Delete SaleDetail
        [HttpDelete("DeleteSaleDetailAsync")]
        public async Task<IActionResult> DeleteSaleDetailAsync(int saleDetailId)
        {
            SaleDetail? saleDetail = await _salesService.GetSaleDetailByIdAsync(saleDetailId);

            if (saleDetail == null)
            {
                return NotFound();
            }

            var deletedSaleDetails = await _salesService.DeleteSaleDetailAsync(saleDetail!);
            return Ok(deletedSaleDetails);
        }

        //DELETE: Delete SaleDetails
        [HttpDelete("DeleteSaleDetailsAsync")]
        public async Task<IActionResult> DeleteSaleDetailsAsync([FromBody] IEnumerable<SaleDetail> saleDetails)
        {
            var deletedSaleDetails = await _salesService.DeleteSaleDetailsAsync(saleDetails);
            return Ok(deletedSaleDetails);
        }
    }
}
