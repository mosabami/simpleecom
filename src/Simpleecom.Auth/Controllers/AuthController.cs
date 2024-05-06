using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models.User;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CosmosDBRepository<User> _repository;

        public AuthController(CosmosDBRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var u = await _repository.AddAsync(user);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string userId)
        {
            var u = await _repository.GetItemsAsync(x => x.Id == userId);
            return Ok(u.FirstOrDefault());
        }


        [HttpGet]
        public async Task<IActionResult> Login(string email)
        {
            var u = await _repository.GetItemsAsync(x => x.Email == email);
            if (u.Count() == 0)
                return NotFound();
            return Ok(u.FirstOrDefault());
        }
    }
}
