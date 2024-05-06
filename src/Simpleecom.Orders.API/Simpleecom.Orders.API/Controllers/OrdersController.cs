using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Simpleecom.Orders.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CosmosDBRepository<Order> _repository;

        public OrdersController(CosmosDBRepository<Order> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserAsync(string userId)
        {
            var orders = await _repository.GetItemsAsync(x => x.UserId == userId);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByIdAsync(string orderId, string userId)
        {
            var orders = await _repository.GetByIdAsync(orderId, userId);
            
            if (orders == null)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsAsync(string orderId)
        {
            var orders = await _repository.GetItemsAsync(x => x.OrderId == orderId);
            if(orders == null)
                return NotFound();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto order)
        {
            await _repository.AddAsync(new Order(order));
            return Ok("Order Created");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderAsync(string orderId)
        {
            await _repository.DeleteAsync(orderId);
            return Ok("Order Deleted");
        }
    }
}
