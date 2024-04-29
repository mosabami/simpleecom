namespace Simpleecom.Shared.Models
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order updatedOrder);
    }
}
