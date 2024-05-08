using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;
using SimpleecomUser = Simpleecom.Shared.Models.User.User;

namespace Simpleecom.Carts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CosmosDBRepository<SimpleecomUser> _repository;
        private readonly CosmosDBRepository<Order> _orderRepository;

        public CartController
            (
            CosmosDBRepository<SimpleecomUser> repository, 
            CosmosDBRepository<Order> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CartCheckout(string userId)
        {
            if (userId != null)
            {
                //Get user cart
                var user = await _repository.GetItemAsync(x => x.Id == userId);

                //Create Order from cart
                if (user?.cart.Products.Count() < 1)
                    return BadRequest("Cart is empty");
                var order = new Order(user.cart);
                await _orderRepository.AddAsync(order);

                //Clear cart
                user.cart = new Cart();
                return Ok(await _repository.UpsertAsync(user));
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var user = await _repository.GetItemAsync(x => x.Id == cart.UserId);
                if (user == null)
                    return NotFound();
                user.cart = cart;
                return Ok( await _repository.UpsertAsync(user));
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
