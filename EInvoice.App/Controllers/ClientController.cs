using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.App.Controllers
{
    [Authorize(Roles = UserRoles.OrganizationAdmin)]
    [Route("[controller]")]
    public class ClientController(IClientService clientService, IMapper mapper) : Controller
    {
        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var client = await clientService.GetById(id);
            return Ok(client);
        }
        [HttpGet("GetByPage/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetByPage(int pageNumber, int pageSize)
        {
            var traits = await clientService.GetByPage(pageNumber, pageSize);
            return Ok(traits);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(ClientFilterDTO filterDTO)
        {
            var clients = await clientService.GetByFilter(filterDTO);
            return Ok(clients);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await clientService.GetAll());
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(ClientDTO clientDTO)
        {
            return Ok(await clientService.Add(clientDTO));
        }
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(ClientDTO clientDTO)
        {
            return Ok(await clientService.Edit(clientDTO));
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await clientService.Delete(id);
            return Ok();
        }
    }
}
