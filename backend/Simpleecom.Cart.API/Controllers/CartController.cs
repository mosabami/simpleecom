using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using Simpleecom.Shared.Models;

namespace Simpleecom.Carts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly IRepository<Cart> _repository;

        public CartController(IRepositoryFactory factory)
        {
            _repository = factory.RepositoryOf<Cart>();
        }


        [HttpGet]
        public IActionResult GetCart(string cartId)
        {
            var carts = _repository.GetAsync(x => x.CartId != cartId);
            return Ok("Cart");
        }
        [HttpGet]
        public IActionResult GetCartById(string cartId)
        {
            var cart = _repository.GetAsync(x => x.CartId == cartId);
            return Ok("Cart");
        }
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var c = _repository.CreateAsync(cart);
            }
            return Ok("Cart");
        }

        [HttpPut]
        public IActionResult UpdateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var u = _repository.UpdateAsync(cart);
            }
            return Ok("Cart");
        }

        [HttpDelete]
        public IActionResult DeleteCart(string cartId)
        {
            var cart = _repository.GetAsync(x => x.CartId == cartId).FirstOrDefaultAsync().Result;
            _repository.DeleteAsync(cart);
            return Ok("Cart");
        }
    }
}
