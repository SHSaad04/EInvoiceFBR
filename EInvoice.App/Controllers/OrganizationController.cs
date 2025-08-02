using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EInvoice.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrganizationController(IOrganizationService organizationService,IUserService userService, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id}")]
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await organizationService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Add(OrganizationDTO organizationDTO)
        {
            var OrganizationRespone = await organizationService.Add(organizationDTO);
            #region User Update
            if (OrganizationRespone != null && OrganizationRespone.Id != 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userService.GetById(Int32.Parse(userId));
                user.OrganizationId = OrganizationRespone.Id;
                await userService.Edit(user);
            }
            #endregion
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Edit(OrganizationDTO organizationDTO)
        {
            return Ok(await organizationService.Edit(organizationDTO));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await organizationService.Delete(id);
            return Ok();
        }
    }
}
