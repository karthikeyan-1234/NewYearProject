using Domain.Contracts;

using Masters.Domain.Contracts;
using Masters.Domain.Entities;
using Masters.Services.DTOs;

using System.Net.WebSockets;

namespace Masters.Services
{
    public class MasterService : IMasterService
    {
        IGenericRepository<Product> _productRepo;
        IGenericRepository<ProductType> _productTypeRepo;
        IGenericRepository<Customer> _customerRepo;
        IMessageService messageService;

        public MasterService(IGenericRepository<Product> productRepo, IGenericRepository<ProductType> productTypeRepo, IGenericRepository<Customer> customerRepo, IMessageService messageService)
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
            _customerRepo = customerRepo;
            this.messageService = messageService;
        }

        //Products

        public async Task DispatchAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            await messageService.PublishMessage(products, "Product.Listing");
        }

        public async Task DispatchAllProductTypesAsync()
        {
            var productTypes = await _productTypeRepo.GetAllAsync();
            await messageService.PublishMessage(productTypes, "ProductType.Listing");
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var result = await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();
            await messageService.PublishMessage(product,"Product.Added");
            return result;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var result = await _productRepo.UpdateAsync(product);
            await _productRepo.SaveChangesAsync();
            return result;
        }

        public async Task<Product> DeleteProductAsync(Product product)
        {
            var result = _productRepo.DeleteAsync(product);
            await _productRepo.SaveChangesAsync();
            return result;
        }


        #region Multiple Products Operations

        public async Task<IEnumerable<Product>> AddProductsAsync(IEnumerable<Product> products)
        {
            var result = await _productRepo.AddRangeAsync(products);
            await _productRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> UpdateProductsAsync(IEnumerable<Product> products)
        {
            var result = await _productRepo.UpdateRangeAsync(products);
            await _productRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> DeleteProductsAsync(IEnumerable<Product> products)
        {
            var result = _productRepo.DeleteRange(products);
            await _productRepo.SaveChangesAsync();
            return result;
        }

        #endregion

        #region Multiple ProductTypes Operations

        public async Task<IEnumerable<ProductType>> AddProductTypesAsync(IEnumerable<ProductType> productTypes)
        {
            var result = await _productTypeRepo.AddRangeAsync(productTypes);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<ProductType>> UpdateProductTypesAsync(IEnumerable<ProductType> productTypes)
        {
            var result = await _productTypeRepo.UpdateRangeAsync(productTypes);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<ProductType>> DeleteProductTypesAsync(IEnumerable<ProductType> productTypes)
        {
            var result = _productTypeRepo.DeleteRange(productTypes);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        #endregion

        #region Multiple Customers Operations

        public async Task<IEnumerable<Customer>> AddCustomersAsync(IEnumerable<Customer> customers)
        {
            var result = await _customerRepo.AddRangeAsync(customers);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Customer>> UpdateCustomersAsync(IEnumerable<Customer> customers)
        {
            var result = await _customerRepo.UpdateRangeAsync(customers);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Customer>> DeleteCustomersAsync(IEnumerable<Customer> customers)
        {
            var result = _customerRepo.DeleteRange(customers);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        //Get Customers by Ids
        public async Task<IEnumerable<Customer>> GetCustomersByIdsAsync(IEnumerable<int> ids)
        {
            var customers = await _customerRepo.GetAllAsync();
            return customers.Where(c => ids.Contains(c.Id)).ToList();
        }

        #endregion

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var result = (from p in _productRepo.Table
                          join pt in _productTypeRepo.Table
                          on p.ProductTypeId equals pt.Id
                          select new ProductDTO
                          {
                              Id = p.Id,
                              Name = p.Name,
                              ProductTypeId = p.ProductTypeId,
                              Category = pt
                          }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetByIdAsync(id);
        }

        //ProductTypes
        public async Task<ProductType> AddProductTypeAsync(ProductType productType)
        {
            var result = await _productTypeRepo.AddAsync(productType);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        public async Task<ProductType> UpdateProductTypeAsync(ProductType productType)
        {
            var result = await _productTypeRepo.UpdateAsync(productType);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        public async Task<ProductType> DeleteProductTypeAsync(ProductType productType)
        {
            var result = _productTypeRepo.DeleteAsync(productType);
            await _productTypeRepo.SaveChangesAsync();
            return result;
        }

        //Get Products by Ids
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids)
        {
            var products = await _productRepo.GetAllAsync();
            return products.Where(p => ids.Contains(p.Id));
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypesAsync()
        {
            var productTypes = await _productTypeRepo.GetAllAsync();

            return productTypes;
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _productTypeRepo.GetByIdAsync(id);
        }

        //Customers
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            var result = await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            var result = await _customerRepo.UpdateAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        public async Task<Customer> DeleteCustomerAsync(Customer customer)
        {
            var result = _customerRepo.DeleteAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByNameAsync(string name)
        {
            return (await _customerRepo.GetAllAsync()).Where(c => c.Name == name);
        }

    }
}
