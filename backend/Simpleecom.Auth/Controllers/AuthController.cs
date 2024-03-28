using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Models.User;

namespace Simpleecom.Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IRepository<User> _repository;

        public AuthController(IRepositoryFactory factory)
        {
            _repository = factory.RepositoryOf<User>();
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            var u = _repository.CreateAsync(user);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetUser(string userId)
        {
            var u = _repository.GetAsync(x => x.Id == userId);
            return Ok(u);
        }

    }
}
