using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Models.User;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IRepository<User> _repository;

        public AuthController(IRepository<User> repository)
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
