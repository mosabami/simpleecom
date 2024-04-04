using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Products.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetProducts(string id)
        {
            var products = _repository.GetByIdAsync(id);
            return Ok(products);
        }
        [HttpGet]
        public IActionResult GetProductById(string id)
        {
            var product = _repository.GetByIdAsync(id);
            return Ok("Product");
        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product != null)
            {
              var p =   _repository.AddAsync(product);
            }
            return Ok("Product");
        }

        [HttpPut(Name = nameof(CreateProduct2))]
        public Task<Product> CreateProduct2([FromBody] Product product) =>
      _repository.AddAsync(product);

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync([FromBody] Product product)
        {
            if (product != null)
            {
                await _repository.UpdateAsync(product.Id, product);
            }

            return Ok("Product");
        }

        [HttpPut(Name = nameof(UpdateProduct2))]
        public Task UpdateProduct2([FromBody] Product product) =>
       _repository.UpdateAsync(product.Id, product);


        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            await _repository.DeleteAsync(id);
            return Ok("Product");
        }
    }
}
