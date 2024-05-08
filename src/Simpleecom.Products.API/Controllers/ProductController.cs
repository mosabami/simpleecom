﻿using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Products.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CosmosDBRepository<Product> _repository;

        public ProductController(CosmosDBRepository<Product> repository)
        {
            _repository = repository;
        }

      
        [HttpGet]
        public IActionResult GetProductByIdBrand(string id, string brand)
        {
            var product = _repository.GetByIdAsync(id, brand);
            return Ok("Product");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(string id)
        {
            var orders = await _repository.GetItemsAsync(x => x.Id == id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto product)
        {
            var existing = await _repository.GetItemsAsync(x => x.ProductId == product.ProductId);
            if (existing.Count() > 0)
                return Conflict("Product already exists");
            if (product != null)
            {
                var p = await _repository.AddAsync(new Product(product));
                return Ok("Product");
            }
            return BadRequest();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync([FromBody] Product product)
        {
            if (product != null)
            {
                await _repository.UpsertAsync( product);
            }

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetItemsAsync(x => x.Id != null);
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            await _repository.DeleteAsync(id);
            return Ok("Product");
        }
    }
}
