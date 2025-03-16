using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Reporting.SalesWebAPI.Services;

using Sales.Reporting.Services;

namespace Sales.Reporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        ISalesReportingService _salesReportingService;
        IPurchaseReportingService _purchaseReportingService;

        public ReportController(ISalesReportingService salesReportingService, IPurchaseReportingService purchaseReportingService)
        {
            _salesReportingService = salesReportingService;
            _purchaseReportingService = purchaseReportingService;
        }

        // GET: api/SaleReport
        [HttpGet("GetSaleReport/{saleId}")]
        public async Task<IActionResult> GetSaleReport(int saleId)
        {
            var saleDetails = await _salesReportingService.GetSaleInfo(saleId);

            return Ok(saleDetails);
        }

        //GET: api/GetAllSales
        [HttpGet("GetAllSales")]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _salesReportingService.GetAllSalesInfo();
            return Ok(sales);
        }

        // GET: api/PurchaseReport
        [HttpGet("GetPurchaseReport/{purchaseId}")]
        public async Task<IActionResult> GetPurchaseReport(int purchaseId)
        {
            var purchaseDetails = await _purchaseReportingService.GetPurchaseInfo(purchaseId);
            return Ok(purchaseDetails);
        }

        // GET: api/GetAllPurchases
        [HttpGet("GetAllPurchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _purchaseReportingService.GetAllPurchasesInfo();
            return Ok(purchases);
        }

    }
}
