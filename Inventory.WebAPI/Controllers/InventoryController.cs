using Inventory.Domain.Entities;
using Inventory.Services.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: Get all inventory entries

        [HttpGet("GetAllInventoryEntries")]
        public async Task<IActionResult> GetAllInventoryEntries()
        {
            var result = await _inventoryService.GetAllInventoryEntries();
            return Ok(result);
        }
    }
}
