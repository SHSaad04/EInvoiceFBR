using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InvoiceController(IInvoiceService invoiceService, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var invoice = await invoiceService.GetById(id);
            return Ok(invoice);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetByPage/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetByPage(int pageNumber, int pageSize)
        {
            var traits = await invoiceService.GetByPage(pageNumber, pageSize);
            return Ok(traits);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(InvoiceFilterDTO filterDTO)
        {
            var invoices = await invoiceService.GetByFilter(filterDTO);
            return Ok(invoices);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await invoiceService.GetAll());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(InvoiceDTO invoiceDTO)
        {
            return Ok(await invoiceService.Add(invoiceDTO));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(InvoiceDTO invoiceDTO)
        {
            return Ok(await invoiceService.Edit(invoiceDTO));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await invoiceService.Delete(id);
            return Ok();
        }
    }
}
