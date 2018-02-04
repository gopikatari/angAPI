using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DatingApp.API.Controllers

{
    [Authorize]
    [Route("api/[Controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _repo;
        public UsersController(IDatingRepository repo,IMapper mapper)
        {
            _repo = repo;
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id){

            var user= await _repo.GetUser(id);
            var user_to_return= Mapper.Map<UserforDetailDTO>(user);
            return Ok(user_to_return);
           
        } 
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(){
            var users=await _repo.GetAllUsers();

            var users_to_return= Mapper.Map<IEnumerable<UserforListDTO>>(users);
            return Ok(users_to_return);
        }
    }
}