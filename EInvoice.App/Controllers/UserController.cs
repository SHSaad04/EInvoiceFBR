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
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController(IUserService userService, IEmailService emailService) : Controller
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

        #region Forgot Password
        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await userService.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate password reset token
                    var token = await userService.GeneratePasswordResetTokenAsync(user);

                    // Encode token for URL safety
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    // Create reset link
                    var callbackUrl = Url.RouteUrl("ResetPassword",
                    new { email = model.Email, token = encodedToken },
                    protocol: HttpContext.Request.Scheme);

                    // Send email
                    await SendPasswordResetEmail(user.Email!, callbackUrl!);
                }

                // Always return the same view to prevent email enumeration
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        private async Task SendPasswordResetEmail(string email, string resetLink)
        {
            var subject = "Reset Your Password";

            // You can create a nice HTML email template
            var message = $@"
            <h2>Password Reset Request</h2>
            <p>You requested to reset your password. Click the link below to proceed:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>If you didn't request this, please ignore this email.</p>
            <p>This link will expire in 1 hour for security reasons.</p>";

            await emailService.SendEmailAsync(email, subject, message);
        }
        // GET: Forgot Password Confirmation
        [HttpGet]
        [AllowAnonymous]
        [Route("ForgotPasswordConfirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPassword", Name = "ResetPassword")]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                return BadRequest("Invalid password reset token");
            }

            var model = new ResetPasswordDTO
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        // POST: Reset Password
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword", Name = "ResetPasswordPost")] // ← Add this
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            // Decode the token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await userService.ResetPasswordAsync(user, decodedToken, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Reset Password Confirmation
        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
    }
}
