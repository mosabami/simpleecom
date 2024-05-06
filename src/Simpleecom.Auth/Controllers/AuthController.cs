using Microsoft.AspNetCore.Mvc;
using Simpleecom.Shared.Models.User;
using Simpleecom.Shared.Repositories;
using System.Reflection.Metadata.Ecma335;

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
        public async Task<IActionResult> RegisterUser(CreateUserDto user)
        {
            var existing = await _repository.GetItemsAsync(x => x.Email == user.Email);
            if (existing.Count() > 0)
                return Conflict("User already exists");
            var u = await _repository.AddAsync(new Shared.Models.User.User(user));
            return Ok(u);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string userId)
        {
            var u = await _repository.GetItemsAsync(x => x.UserId == userId);
            if (u.Count() == 0)
                return NotFound();
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
