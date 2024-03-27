using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using Simpleecom.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Simpleecom.Orders.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repository;

        public OrdersController(IRepositoryFactory factory)
        {
            _repository = factory.RepositoryOf<Order>();
        }

        [HttpGet]
        public IActionResult GetOrdersByUser(string userId)
        {
            var orders = _repository.GetAsync(x => x.UserId != userId);
            return Ok(orders);
        }

        [HttpGet("{id:string}")]
        public IActionResult GetOrdersById(string orderId)
        {
            var orders = _repository.GetAsync(x => x.OrderId != orderId);
            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateOrderAsync([FromBody] Order value)
        {
            _repository.CreateAsync(value);
            return Ok("Order Created");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string orderId)
        {
            _repository.DeleteAsync(orderId);
            return Ok("Order Deleted");
        }
    }
}
