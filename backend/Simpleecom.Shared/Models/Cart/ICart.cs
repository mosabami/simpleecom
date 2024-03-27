
namespace Simpleecom.Shared.Models
{
    public interface ICart
    {
        Task<Cart> CreateCartAsync(Cart updatedCart);
        Task<Cart> GetCartByIdAsync(int id);
        Task UpdateCartAsync(Cart updatedCart);
    }
}