using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Models.User;
using Simpleecom.Shared.Repositories;
using System.Runtime.CompilerServices;

namespace Simpleecom.Carts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CosmosDBRepository<Shared.Models.User.User> _repository;
        private readonly CosmosDBRepository<Order> _orderRepository;

        public CartController
            (
            CosmosDBRepository<Shared.Models.User.User> repository, 
            CosmosDBRepository<Order> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CartCheckout([FromBody] Cart cart)
        {
            if (cart != null)
            {
                //Create Order from cart
                var order = new Order(cart);
                await _orderRepository.AddAsync(order);

                //Clear cart
                var user = await _repository.GetItemsAsync(x => x.Id == cart.UserId);
                var u = user.FirstOrDefault();
                u.cart = new Cart();
                return Ok(await _repository.UpsertAsync(u));
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var user = await _repository.GetItemsAsync(x => x.Id == cart.UserId);
                var u = user.FirstOrDefault();
                u.cart = cart;
                return Ok( await _repository.UpsertAsync(u));
            }
            return BadRequest();
        }

        
        [HttpDelete]
        public async Task<IActionResult> DeleteCartAsync(string userId)
        {
            var user = await _repository.GetItemsAsync(x => x.Id == userId);
            var u = user.FirstOrDefault();
            u.cart =new Cart();
            return Ok(await _repository.UpsertAsync(u));
        }
    }
}
