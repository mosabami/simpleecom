using Simpleecom.Shared.Models;

namespace Simpleecom.Products.API.Services
{
    public class ProductService 
    {
        public ProductService() { }

        public ProductService(string name) { }

        public Task<Product> CreateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
