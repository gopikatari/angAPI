using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            //validate the data

            if (await _repo.IsExists(username))
                return BadRequest("UserName already Taken");

            var usertocreate = new User
            {
                UserName = username
            };

            var createUser= await _repo.Register(usertocreate,password);
            return StatusCode(201);


    }
}
}