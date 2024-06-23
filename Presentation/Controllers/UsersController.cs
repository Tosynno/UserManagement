using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        protected IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("account/User")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
        {
            var result = await _userService.CreateUser(user);
            if (result == "User created successful")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("account/get-User")]
        public async Task<IActionResult> getUser()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("account/get-User/{ReferenceId}")]
        public async Task<IActionResult> getUser(string ReferenceId)
        {
            var result = await _userService.GetUserByIdAsync(ReferenceId);
            return Ok(result);
        }

        [HttpPut("account/user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updatedUser)
        {
            var result = await _userService.UpdateUser(updatedUser);
            if (result == "User Updated successfully")
            {
                return Ok(result);
            }
            return NotFound(result);
            
        }

        [HttpGet("account/{ReferenceId}/deactivate-User")]
        public async Task<IActionResult> DeactivateUser(string ReferenceId)
        {
            var result = await _userService.DeactivateUser(ReferenceId);
            if (result == "User Updated successfully")
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
