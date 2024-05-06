using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Carts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CosmosDBRepository<Cart> _repository;

        public CartController(CosmosDBRepository<Cart> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            var carts = await _repository.GetItemsAsync(x => x.CartId == cartId);
            if (carts == null)
                return NotFound();
            return Ok(carts.FirstOrDefault());
        }

        [HttpGet]
        public IActionResult GetCartById(string id)
        {
            var cart = _repository.GetItemsAsync(x => x.Id == id);
            if (cart == null)   
                return NotFound();
            return Ok("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto cart)
        {
            if (cart != null)
            {
               await _repository.AddAsync(new Cart(cart));
            }
            return Ok("Cart");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCartAsync([FromBody] Cart cart, string userId)
        {
            if (cart != null)
            {
                await _repository.UpdateAsync(cart.Id, cart, userId);
            }
            return Ok("Cart");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCartAsync(string id)
        {
            await _repository.DeleteAsync(id);
            return Ok("Item deleted");
        }
    }
}
