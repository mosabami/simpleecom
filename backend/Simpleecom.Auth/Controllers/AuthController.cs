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
        public async Task<IActionResult> RegisterUserAsync(User user)
        {
            var u = await _repository.AddAsync(user);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var u = await _repository.GetItemsAsync(x => x.Id == userId);
            return Ok(u.FirstOrDefault());
        }
    }
}
