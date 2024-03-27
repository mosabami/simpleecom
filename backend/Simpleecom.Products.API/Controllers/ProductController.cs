using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using Simpleecom.Shared.Models;

namespace Simpleecom.Products.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductController(IRepositoryFactory factory)
        {
            _repository = factory.RepositoryOf<Product>();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _repository.GetAsync(x => x.Id != null);
            return Ok(products);
        }
        [HttpGet]
        public IActionResult GetProductById(string id)
        {
            var product = _repository.GetAsync(x => x.Id == id);
            return Ok("Product");
        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product != null)
            {
              var p =   _repository.CreateAsync(product);
            }
            return Ok("Product");
        }

        [HttpPut(Name = nameof(CreateProduct2))]
        public ValueTask<Product> CreateProduct2([FromBody] Product product) =>
      _repository.CreateAsync(product);

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            if (product != null)
            {
               var u = _repository.UpdateAsync(product);
            }

            return Ok("Product");
        }

        [HttpPut(Name = nameof(UpdateProduct2))]
        public ValueTask<Product> UpdateProduct2([FromBody] Product product) =>
       _repository.UpdateAsync(product);


        [HttpDelete]
        public IActionResult DeleteProduct(string id)
        {
            _repository.DeleteAsync(_repository.GetAsync(x => x.Id == id).Result.First());
            return Ok("Product");
        }
    }
}
