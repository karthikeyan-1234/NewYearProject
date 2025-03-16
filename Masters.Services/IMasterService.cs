using Masters.Domain.Entities;
using Masters.Services.DTOs;

namespace Masters.Services
{
    public interface IMasterService
    {
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Product> AddProductAsync(Product product);
        Task<ProductType> AddProductTypeAsync(ProductType productType);
        Task<Customer> DeleteCustomerAsync(Customer customer);
        Task<Product> DeleteProductAsync(Product product);
        Task<ProductType> DeleteProductTypeAsync(ProductType productType);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductType>> GetAllProductTypesAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersByNameAsync(string name);
        Task<IEnumerable<Customer>> GetCustomersByIdsAsync(IEnumerable<int> ids);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids);
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Product> UpdateProductAsync(Product product);
        Task<ProductType> UpdateProductTypeAsync(ProductType productType);
        Task DispatchAllProductsAsync();
        Task DispatchAllProductTypesAsync();


        #region Multiple Products CRUD Operations

        Task<IEnumerable<Product>> AddProductsAsync(IEnumerable<Product> products);
        Task<IEnumerable<Product>> UpdateProductsAsync(IEnumerable<Product> products);
        Task<IEnumerable<Product>> DeleteProductsAsync(IEnumerable<Product> products);

        #endregion

        #region Multiple ProductTypes CRUD Operations

        Task<IEnumerable<ProductType>> AddProductTypesAsync(IEnumerable<ProductType> productTypes);
        Task<IEnumerable<ProductType>> UpdateProductTypesAsync(IEnumerable<ProductType> productTypes);
        Task<IEnumerable<ProductType>> DeleteProductTypesAsync(IEnumerable<ProductType> productTypes);

        #endregion

        #region Multiple Customers Operations

        Task<IEnumerable<Customer>> AddCustomersAsync(IEnumerable<Customer> customers);
        Task<IEnumerable<Customer>> UpdateCustomersAsync(IEnumerable<Customer> customers);
        Task<IEnumerable<Customer>> DeleteCustomersAsync(IEnumerable<Customer> customers);

        #endregion

    }
}