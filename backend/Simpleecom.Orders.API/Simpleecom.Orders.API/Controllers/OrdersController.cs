using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Simpleecom.Orders.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repository;

        public OrdersController(IRepository<Order> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetOrdersByUser(string userId)
        {
            var orders = _repository.GetByIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByIdAsync(string orderId)
        {
            var orders = await _repository.GetByIdAsync(orderId);        
                return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsAsync(string orderId)
        {
            var orders = await _repository.GetItemsAsync(x => x.OrderId != orderId);
            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateOrderAsync([FromBody] Order value)
        {
            _repository.AddAsync(value);
            return Ok("Order Created");
        }

        [HttpDelete]
        public IActionResult DeleteOrder(string orderId)
        {
            _repository.DeleteAsync(orderId);
            return Ok("Order Deleted");
        }
    }
}
