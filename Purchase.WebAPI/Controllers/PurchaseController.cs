using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Purchases.Domain.Entities;
using Purchases.Services;

namespace Purchases.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        IPurchaseService _PurchasesService;

        public PurchaseController(IPurchaseService PurchasesService)
        {
            _PurchasesService = PurchasesService;
        }

        #region Purchase Master CRUD Operations

        // POST: Add new Purchase
        [HttpPost("AddPurchaseAsync")]
        public async Task<IActionResult> AddPurchaseAsync([FromBody] Purchase Purchase)
        {
            await _PurchasesService.AddPurchaseAsync(Purchase);
            return CreatedAtAction("AddPurchase", Purchase);
        }


        // GET: Get all Purchases
        [HttpGet("GetAllPurchasesAsync")]
        public async Task<IActionResult> GetAllPurchasesAsync()
        {
            var Purchases = await _PurchasesService.GetAllPurchasesAsync();
            return Ok(Purchases);
        }

        // PUT: Update Purchase
        [HttpPut("UpdatePurchaseAsync")]
        public async Task<IActionResult> UpdatePurchaseAsync([FromBody] Purchase Purchase)
        {
            await _PurchasesService.UpdatePurchaseAsync(Purchase);
            return NoContent();
        }

        // DELETE: Delete Purchase
        [HttpDelete("DeletePurchaseAsync")]
        public async Task<IActionResult> DeletePurchaseAsync([FromBody] Purchase Purchase)
        {
            await _PurchasesService.DeletePurchaseAsync(Purchase);
            return NoContent();
        }

        // DELETE: Delete Purchase by Id
        [HttpDelete("DeletePurchaseByIdAsync/{id}")]
        public async Task<IActionResult> DeletePurchaseByIdAsync(int id)
        {
            var Purchase = await _PurchasesService.GetPurchaseByIdAsync(id);
            await _PurchasesService.DeletePurchaseAsync(Purchase);
            return NoContent();
        }

        #endregion

        #region Single PurchaseDetail CRUD Operations

        // POST: Add new PurchaseDetail
        [HttpPost("AddPurchaseDetailAsync")]
        public async Task<IActionResult> AddPurchaseDetailAsync([FromBody] PurchaseDetail PurchaseDetail)
        {
            await _PurchasesService.AddPurchaseDetailAsync(PurchaseDetail);
            return CreatedAtAction("AddPurchaseDetail", PurchaseDetail);
        }

        // PUT: Update PurchaseDetail
        [HttpPut("UpdatePurchaseDetailAsync")]
        public async Task<IActionResult> UpdatePurchaseDetailAsync([FromBody] PurchaseDetail PurchaseDetail)
        {
            await _PurchasesService.UpdatePurchaseDetailAsync(PurchaseDetail);
            return NoContent();
        }

        // DELETE: Delete PurchaseDetail
        [HttpDelete("DeletePurchaseDetailAsync")]
        public async Task<IActionResult> DeletePurchaseDetailAsync([FromBody] PurchaseDetail PurchaseDetail)
        {
            await _PurchasesService.DeletePurchaseDetailAsync(PurchaseDetail);
            return NoContent();
        }

        // GET: Get PurchaseDetail by Id
        [HttpGet("GetPurchaseDetailByIdAsync/{id}")]
        public async Task<IActionResult> GetPurchaseDetailByIdAsync(int id)
        {
            var PurchaseDetail = await _PurchasesService.GetPurchaseDetailByIdAsync(id);
            return Ok(PurchaseDetail);
        }

        #endregion

        #region Multiple PurchaseDetails CRUD Operations

        // POST: Add new PurchaseDetails

        [HttpPost("AddPurchaseDetailsAsync")]
        public async Task<IActionResult> AddPurchaseDetailsAsync([FromBody] IEnumerable<PurchaseDetail> PurchaseDetails)
        {


            await _PurchasesService.AddPurchaseDetailsAsync(PurchaseDetails);
            return CreatedAtAction("AddPurchaseDetails", PurchaseDetails);
        }

        // PUT: Update PurchaseDetails
        [HttpPut("UpdatePurchaseDetailsAsync")]
        public async Task<IActionResult> UpdatePurchaseDetailsAsync([FromBody] IEnumerable<PurchaseDetail> PurchaseDetails)
        {
            await _PurchasesService.UpdatePurchaseDetailsAsync(PurchaseDetails);
            return NoContent();
        }

        // DELETE: Delete PurchaseDetails
        [HttpDelete("DeletePurchaseDetailsAsync")]
        public async Task<IActionResult> DeletePurchaseDetailsAsync([FromBody] IEnumerable<PurchaseDetail> PurchaseDetails)
        {
            await _PurchasesService.DeletePurchaseDetailsAsync(PurchaseDetails);
            return NoContent();
        }

        #endregion

        #region Others            

        // GET: Get Purchase by Id
        [HttpGet("GetPurchaseByIdAsync/{id}")]
        public async Task<IActionResult> GetPurchaseByIdAsync(int id)
        {
            var Purchase = await _PurchasesService.GetPurchaseByIdAsync(id);
            return Ok(Purchase);
        }        

        // GET: Get all PurchaseDetails
        [HttpGet("GetAllPurchaseDetailsAsync")]
        public async Task<IActionResult> GetAllPurchaseDetailsAsync()
        {
            var PurchaseDetails = await _PurchasesService.GetAllPurchaseDetailsAsync();
            return Ok(PurchaseDetails);
        }

        //GET : Get all PurchaseDetails by PurchaseId
        [HttpGet("GetPurchaseDetailsForPurchase/{id}")]
        public IActionResult GetPurchaseDetailsForPurchase(int id)
        {
            var PurchaseDetails = _PurchasesService.GetPurchaseDetailsForPurchase(id);
            return Ok(PurchaseDetails);
        }




        #endregion
    }
}
