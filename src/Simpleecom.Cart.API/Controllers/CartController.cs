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

        

        [HttpPost]
        public async Task<IActionResult> UpsertCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
              return Ok( await _repository.UpsertAsync(cart));
            }
            return BadRequest();
        }

        

        [HttpDelete]
        public async Task<IActionResult> DeleteCartAsync(string id)
        {
            await _repository.DeleteAsync(id);
            return Ok("Item deleted");
        }
    }
}
