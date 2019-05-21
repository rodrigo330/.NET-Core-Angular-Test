using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using web.api.Data;
using web.api.Dtos;
using web.api.Models;

namespace web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public IAuthRepository _Repo { get; }
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
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

        

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userLoginDto)
        {
            var userFromRepo = await _Repo.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token)});
        }
    }
}