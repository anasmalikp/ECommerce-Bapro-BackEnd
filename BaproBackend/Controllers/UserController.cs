using BaproBackend.Data.DTO;
using BaproBackend.Data.Encryption;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaproBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var result = await services.Register(user);
            if(result)
            {
                return Ok("Registered");
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login cred)
        {
            var token = await services.Login(cred);
            if (token == null)
            {
                return BadRequest("something went wrong");
            }
            var userName = PasswordHasher.TokenDecode(token);
            return Ok(new { Token = token, username = userName.username });
        }

        
    }
}
