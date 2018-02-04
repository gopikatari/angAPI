using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO userForRegisterDTO)
        {
            //validate the data
            if(!string.IsNullOrEmpty(userForRegisterDTO.username))
                userForRegisterDTO.username = userForRegisterDTO.username.ToLower();
            if (await _repo.IsExists(userForRegisterDTO.username))
                ModelState.AddModelError("UserName", "UserName already Exists");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usertocreate = new User
            {
                UserName = userForRegisterDTO.username
            };

            var createUser = await _repo.Register(usertocreate, userForRegisterDTO.password);
            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginForUserDTO loginForUserDTO)
        {
            //throw new Exception("really says No!");
                var userFromRepo = await _repo.Login(loginForUserDTO.username.ToLower(), loginForUserDTO.password);
               
                if (userFromRepo == null)
                {
                    //return Unauthorized();
                    //ModelState.AddModelError("","Invalid credentials");
                    return BadRequest(ModelState);
                }

                //generate Token

                var tokenhandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
                var tokendescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName)
                }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenhandler.CreateToken(tokendescriptor);
                var tokenString = tokenhandler.WriteToken(token);

                return Ok(new { tokenString });

        
        }
    }
}