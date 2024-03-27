namespace Simpleecom.Shared.Models
{
    public interface IProduct
    {
        Task<Product> CreateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task UpdateProductAsync(Product updatedProduct);
    }
}