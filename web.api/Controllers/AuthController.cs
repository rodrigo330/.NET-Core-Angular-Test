using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.api.Data;
using web.api.Dtos;
using web.api.Models;

namespace web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _Repo { get; }
        public AuthController(IAuthRepository repo)
        {
            _Repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto UserForRegisterDto)
        {
            UserForRegisterDto.Username = UserForRegisterDto.Username.ToLower();

            if ( await _Repo.UserExists(UserForRegisterDto.Username) )
                return BadRequest("Username aready exists");

            var userToCreate = new User
            {
                Username = UserForRegisterDto.Username
            };

            var createdUser = await _Repo.Register(userToCreate, UserForRegisterDto.Password);

            return StatusCode(201);
        }



    }
}