using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace EInvoice.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrganizationController(IOrganizationService organizationService, IMapper mapper) : ControllerBase
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
            return Ok(await organizationService.Add(organizationDTO));
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
