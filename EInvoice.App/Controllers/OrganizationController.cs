using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Index()
        {
            return View(await organizationService.GetAll());
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
                var user = await userService.GetById(userId);
                user.OrganizationId = OrganizationRespone.Id;
                await userService.Edit(user);
                bool result = await userService.UpdateClaims(user);
            }
            #endregion
            return RedirectToAction("Index");
        }
        [HttpGet("Upsert/{id?}")]
        public async Task<IActionResult> Upsert(long? id)
        {
            if (id == null || id == 0) // Add
                return View(new OrganizationDTO());

            // Edit
            var org = await organizationService.GetById(id.Value);
            if (org == null)
                return NotFound();

            return View(org);
        }

        [HttpPost("Upsert/{id?}")]
        public async Task<IActionResult> Upsert(long? id, OrganizationDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (id == null || id == 0) // ADD logic
            {
                var organizationResponse = await organizationService.Add(model);

                if (organizationResponse != null && organizationResponse.Id != 0)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await userService.GetById(userId);

                    user.OrganizationId = organizationResponse.Id;
                    await userService.Edit(user);

                    await userService.UpdateClaims(user);
                }
            }
            else // EDIT logic
            {
                model.Id = id.Value;
                await organizationService.Edit(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Delete(long id)
        {
            var org = await organizationService.GetById(id);
            if (org == null)
                return NotFound();

            return View(org);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await organizationService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet("Details")]
        [Authorize(Roles = UserRoles.SuperAdminANDOrganizationAdmin)]
        public async Task<IActionResult> Details()
        {
            long id = 0;
            var org = await organizationService.GetById(id);
            if (org == null)
                return NotFound();

            return View(org);
        }




        #region API METHODS
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
        public async Task<IActionResult> DeleteOLD(long id)
        {
            await organizationService.Delete(id);
            return Ok();
        }
        #endregion
    }
}
