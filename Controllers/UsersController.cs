using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await _repo.GetUser(id);
            var user_to_return = _mapper.Map<UserforDetailDTO>(user);
            return Ok(user_to_return);

        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetAllUsers();

            var users_to_return = _mapper.Map<IEnumerable<UserforListDTO>>(users);
            return Ok(users_to_return);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDTO userforupdateDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userfromRepo = await _repo.GetUser(id);
            if (userfromRepo == null)
            {
                return NotFound($"Could not find the user with the id of {id}");
            }

            if (currentUser != userfromRepo.Id)
            {
                return Unauthorized();

            }
            _mapper.Map(userforupdateDTO, userfromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user on {id} failed on Save!");

        }
    }
}