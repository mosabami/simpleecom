﻿using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Carts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly IRepository<Cart> _repository;

        public CartController(IRepository<Cart> repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            var carts = await _repository.GetItemsAsync(x => x.CartId == cartId);
            return Ok(carts.FirstOrDefault());
        }

        [HttpGet]
        public IActionResult GetCartById(string id)
        {
            var cart = _repository.GetItemsAsync(x => x.Id == id);
            return Ok("Cart");
        }
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var c = _repository.AddAsync(cart);
            }
            return Ok("Cart");
        }

        [HttpPut]
        public IActionResult UpdateCart([FromBody] Cart cart)
        {
            if (cart != null)
            {
                var u = _repository.UpdateAsync(cart.Id,cart);
            }
            return Ok("Cart");
        }

        [HttpDelete]
        public IActionResult DeleteCart(string id)
        {
            _repository.DeleteAsync(id);
            return Ok("Cart");
        }
    }
}
