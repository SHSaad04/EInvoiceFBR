using EInvoice.Common.DTO;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
using EInvoice.Common.Exceptions.Types;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController(IUserService userService) : Controller
    {
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticateRequestDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await userService.Authenticate(model);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(model);
            }
            return response.IsOrganizationAssociated
                ? RedirectToAction("Details", "Organization")
                : RedirectToAction("Upsert", "Organization");
        }
        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {

                var userId = await userService.Signup(model);
                if (userId == null)
                {
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                    return View(model);
                }
                return RedirectToAction("Login", "Users");
            }
            catch (UserTypeException ex)
            {
                // Split errors and add them to ModelState
                foreach (var error in ex.Message.Split('\n'))
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }
        }
        [AllowAnonymous]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await userService.Signout(userId);
            return RedirectToAction("Login", "Users");
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

        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        { 
            return View();
        }
            [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal if the email exists to avoid enumeration attacks
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await userService.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Users",
                new { token, email = user.Email }, Request.Scheme);

            // Send email here (SMTP or log for testing)
            Console.WriteLine("Reset Password Link: " + resetLink);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordDTO { Token = token, Email = email });
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await userService.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

    }
}
