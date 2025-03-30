using Masters.Domain.Entities;
using Masters.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OIDC;

using System.Text.Json;

namespace Sales.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CheckIf(resource: "masters")]
    public class MastersController : ControllerBase
    {
        IMasterService masterService;
        ILogger<MastersController> logger;

        public MastersController(IMasterService masterService, ILogger<MastersController> logger)
        {
            this.masterService = masterService;
            this.logger = logger;
        }


        [HttpGet("getAllProducts")]
        [CheckIf(resource: "masters",hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetAllProducts()
        {
            logger.LogInformation("Getting all products");
            var products = await masterService.GetAllProductsAsync();
            logger.LogInformation(JsonSerializer.Serialize(products));
            return Ok(products);
        }

        // PUT: api/Masters/updateProduct
        [HttpPut("updateProduct")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            await masterService.UpdateProductAsync(product);
            return Ok();
        }

        // PUT : api/Masters/updateProducts

        [HttpPut("updateProducts")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateProducts([FromBody] IEnumerable<Product> products)
        {
            await masterService.UpdateProductsAsync(products);
            return Ok();
        }

        // POST : api/Masters/addProducts

        [HttpPost("addProducts")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddProducts([FromBody] IEnumerable<Product> products)
        {
            var newProducts = await masterService.AddProductsAsync(products);
            return Ok(newProducts);
        }

        // POST : api/Masters/addProductTypes

        [HttpPost("addProductTypes")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddProductTypes([FromBody] IEnumerable<ProductType> productTypes)
        {
            var newProductTypes = await masterService.AddProductTypesAsync(productTypes);
            return Ok(newProductTypes);
        }

        // POST : api/Masters/addCustomers

        [HttpPost("addCustomers")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddCustomers([FromBody] IEnumerable<Customer> customers)
        {
            var newCustomers = await masterService.AddCustomersAsync(customers);
            return Ok(newCustomers);
        }



        // PUT : api/Masters/updateProductTypes

        [HttpPut("updateProductTypes")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateProductTypes([FromBody] IEnumerable<ProductType> productTypes)
        {
            await masterService.UpdateProductTypesAsync(productTypes);
            return Ok();
        }

        // PUT : api/Masters/updateCustomers

        [HttpPut("updateCustomers")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateCustomers([FromBody] IEnumerable<Customer> customers)
        {
            await masterService.UpdateCustomersAsync(customers);
            return Ok();
        }

        // DELETE : api/Masters/deleteProducts

        [HttpDelete("deleteProducts")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteProducts([FromBody] IEnumerable<Product> products)
        {
            await masterService.DeleteProductsAsync(products);
            return Ok();
        }

        // DELETE : api/Masters/deleteProductTypes

        [HttpDelete("deleteProductTypes")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteProductTypes([FromBody] IEnumerable<ProductType> productTypes)
        {
            await masterService.DeleteProductTypesAsync(productTypes);
            return Ok();
        }

        // DELETE : api/Masters/deleteCustomers

        [HttpDelete("deleteCustomers")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteCustomers([FromBody] IEnumerable<Customer> customers)
        {
            await masterService.DeleteCustomersAsync(customers);
            return Ok();
        }

        [HttpGet("getAllProductTypes")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var productTypes = await masterService.GetAllProductTypesAsync();
            return Ok(productTypes);
        }

        // GET : api/Masters/getAllCustomers

        [HttpGet("getAllCustomers")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await masterService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET : api/Masters/getCustomerById/5

        [HttpGet("getCustomerById/{id}")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await masterService.GetCustomerByIdAsync(id);
            return Ok(customer);
        }

        //POST : api/Masters/getCustomersByIds

        [HttpPost("getCustomersByIds")]
        //[CheckIf(resource: "masters", hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetCustomersByIds([FromBody] IEnumerable<int> ids)
        {
            var customers = await masterService.GetCustomersByIdsAsync(ids);
            return Ok(customers);
        }



        #region Others

        // POST: api/Masters/addProduct
        [HttpPost("addProduct")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var newProduct = await masterService.AddProductAsync(product);
            return Ok(newProduct);
        }

        // POST: api/Masters/addProductType
        [HttpPost("addProductType")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddProductType([FromBody] ProductType productType)
        {
            var newProductType = await masterService.AddProductTypeAsync(productType);
            return Ok(newProductType);
        }

       


        // GET: api/Masters/getProductsByIds
        [HttpPost("getProductsByIds")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Read])]
        public async Task<IActionResult> GetProductsByIds([FromBody] IEnumerable<int> ids)
        {
            var products = await masterService.GetProductsByIdsAsync(ids);
            return Ok(products);
        }




        // PUT: api/Masters/updateProductType
        [HttpPut("updateProductType")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateProductType([FromBody] ProductType productType)
        {
            await masterService.UpdateProductTypeAsync(productType);
            return Ok();
        }

        // DELETE: api/Masters/deleteProduct/5
        [HttpDelete("deleteProduct/{id}")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await masterService.GetProductByIdAsync(id);
                await masterService.DeleteProductAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Masters/deleteProductType/5
        [HttpDelete("deleteProductType/{id}")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            var productType = await masterService.GetProductTypeByIdAsync(id);
            await masterService.DeleteProductTypeAsync(productType);
            return Ok();
        }


        //GET, POST, PUT, DELETE for Customers
        // POST: api/Masters/addCustomer
        [HttpPost("addCustomer")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Create])]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            var newCustomer = await masterService.AddCustomerAsync(customer);
            return Ok(newCustomer);
        }


        // PUT: api/Masters/updateCustomer
        [HttpPut("updateCustomer")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Edit])]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
        {
            await masterService.UpdateCustomerAsync(customer);
            return Ok();
        }

        // DELETE: api/Masters/deleteCustomer/5
        [HttpDelete("deleteCustomer/{id}")]
        [CheckIf(resource: "masters", hasScopes: [Permissions.Delete])]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await masterService.GetCustomerByIdAsync(id);
            await masterService.DeleteCustomerAsync(customer);
            return Ok();
        }

        #endregion
    }
}
