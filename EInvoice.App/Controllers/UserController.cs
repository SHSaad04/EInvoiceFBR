using EInvoice.Common.DTO;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController(IUserService userService) : Controller
    {
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserDTO userDTO)
        {
            var userId = await userService.Signup(userDTO);
            return Ok();
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDTO request)
        {
            var response = await userService.Authenticate(request);
            if (response == null)
                return Unauthorized("Invalid credentials");

            return Ok(response);
        }

        [HttpPost("Signoff")]
        [AllowAnonymous]
        public async Task<IActionResult> Signout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await userService.Signout(userId);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAll();
            return Ok(users);
        }
        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(UserFilterDTO filterDTO)
        {
            var users = await userService.GetByFilter(filterDTO);
            return Ok(users);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserDTO dto)
        {
            var user = await userService.Edit(dto);
            return Ok(user);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await userService.Delete(id);
            return NoContent();
        }
    }
}
