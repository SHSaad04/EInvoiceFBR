using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClientController(IClientService clientService, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var client = await clientService.GetById(id);
            return Ok(client);
        }
        [Authorize(Roles = "Admin")]
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await clientService.GetAll());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ClientDTO clientDTO)
        {
            return Ok(await clientService.Add(clientDTO));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(ClientDTO clientDTO)
        {
            return Ok(await clientService.Edit(clientDTO));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await clientService.Delete(id);
            return Ok();
        }
    }
}
