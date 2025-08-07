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
        public async Task<IActionResult> Index()
        {
            return View(await clientService.GetAll());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var client = await clientService.GetById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        [HttpGet("Upsert/{id?}")]
        public async Task<IActionResult> Upsert(long? id)
        {
            if (id.HasValue)
            {
                var client = await clientService.GetById(id.Value);
                if (client == null)
                    return NotFound();
                return View(client);
            }
            return View(new ClientDTO());
        }

        [HttpPost("Upsert/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ClientDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await clientService.Add(model);
            else
                await clientService.Edit(model);

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var client = await clientService.GetById(id);
            if (client == null)
                return NotFound();
            await clientService.Delete(id);
            return RedirectToAction("Index");
        }

        #region API Methods
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
        public async Task<IActionResult> DeleteAPI(long id)
        {
            await clientService.Delete(id);
            return Ok();
        }
        #endregion
    }
}
