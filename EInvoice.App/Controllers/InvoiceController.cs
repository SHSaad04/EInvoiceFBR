using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Common.ViewModel;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.OrganizationAdmin)]
    public class InvoiceController(IInvoiceService invoiceService,IClientService clientService,IProductService productService, IMapper mapper) : Controller
    {
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await invoiceService.GetAll());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var client = await invoiceService.GetById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add(long? id)
        {
            InvoiceViewModel model = new InvoiceViewModel();
            model.Clients = await clientService.GetAll();
            model.Products = await productService.GetAll();
            model.InvoiceDate = DateTime.Now;
            return View(model);
        }
        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(InvoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns because they'll be null on postback
                model.Clients = await clientService.GetAll();
                model.Products = await productService.GetAll();
                return View(model);
            }

            // Save invoice
            //await invoiceService.Add(model);

            return RedirectToAction("Index");
        }
        [HttpPost("Upsert/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(InvoiceDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await invoiceService.Add(model);
            else
                await invoiceService.Edit(model);

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var client = await invoiceService.GetById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await invoiceService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
