using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.OrganizationAdmin)]
    public class OrganizationController(IOrganizationService organizationService,IUserService userService, IMapper mapper) : Controller
    {
        [AllowAnonymous]
        [HttpGet("DebugClaims")]
        public IActionResult DebugClaims()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Json(new { message = "No claims found or user not authenticated" });
            }

            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Json(claims);
        }
        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("Add")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(OrganizationDTO organizationDTO)
        {
            if (!ModelState.IsValid)
                return View(organizationDTO);
            var OrganizationRespone = await organizationService.Add(organizationDTO);
            #region User Update
            if (OrganizationRespone != null && OrganizationRespone.Id != 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userService.GetById(long.Parse(userId));
                user.OrganizationId = OrganizationRespone.Id;
                await userService.Edit(user);
                bool result = await userService.UpdateClaims(user);
            }
            #endregion
            return RedirectToAction("Index");
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            //var orgIdClaim = User.FindFirst("OrganizationId")?.Value;
            var question = await organizationService.GetById(id);
            return Ok(question);
        }
        [HttpGet("GetByPage/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetByPage(int pageNumber, int pageSize)
        {
            var traits = await organizationService.GetByPage(pageNumber, pageSize);
            return Ok(traits);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(OrganizationFilterDTO filterDTO)
        {
            var questions = await organizationService.GetByFilter(filterDTO);
            return Ok(questions);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await organizationService.GetAll());
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(OrganizationDTO organizationDTO)
        {
            return Ok(await organizationService.Edit(organizationDTO));
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await organizationService.Delete(id);
            return Ok();
        }
    }
}
