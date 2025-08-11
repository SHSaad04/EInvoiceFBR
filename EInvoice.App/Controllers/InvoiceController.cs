using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
using EInvoice.Common.ViewModel;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Helpers;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EInvoice.App.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = UserRoles.OrganizationAdmin)]
    public class InvoiceController(IInvoiceService invoiceService, IClientService clientService, IProductService productService, IOrganizationService organizationService, IMapper mapper, IHttpContextAccessor httpContextAccessor) : Controller
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

        [HttpGet]
        public async Task<IActionResult> Add(long? id)
        {
            InvoiceDTO model = new InvoiceDTO();
            model.Clients = await clientService.GetAll();
            model.ProductViewModel = new ProductViewModel();
            model.ProductViewModel.Products = await productService.GetDropdown();
            model.InvoiceTypes = await invoiceService.GetAllInvocieTypes();
            model.InvoiceDate = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(InvoiceDTO model, string InvoiceItemsJson)
        {
            if (!string.IsNullOrEmpty(InvoiceItemsJson))
            {
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                model.InvoiceItems = JsonSerializer.Deserialize<List<InvoiceItemDTO>>(InvoiceItemsJson, options);
            }
            #region Validation
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns because they'll be null on postback
                model.Clients = await clientService.GetAll();
                model.ProductViewModel = new ProductViewModel();
                model.ProductViewModel.Products = await productService.GetDropdown();
                model.InvoiceTypes = await invoiceService.GetAllInvocieTypes();
                // Set a flag for JS to detect
                ViewBag.ShowValidationModal = true;
                return View(model);
            }
            #endregion

            #region Map Seller Data based on Organization_id
            long? OrganizationId = httpContextAccessor.HttpContext?.User.GetOrganizationId();
            OrganizationDTO seller = await organizationService.GetById(OrganizationId.Value);
            model.SellerId = seller.Id;
            model.SellerNTNCNIC = seller.NTNCNIC;
            model.SellerBusinessName = seller.BusinessName;
            model.SellerProvince = seller.Province;
            model.SellerAddress = seller.Address;
            #endregion

            #region Map Client Data based on ClientId
            ClientDTO client = await clientService.GetById(model.BuyerId);
            model.BuyerId = client.Id;
            model.BuyerNTNCNIC = client.NTNCNIC;
            model.BuyerBusinessName = client.BusinessName;
            model.BuyerProvince = client.Province;
            model.BuyerAddress = client.Address;
            model.BuyerRegistrationType = client.RegistrationType;
            #endregion

            await invoiceService.Add(model);
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
            await invoiceService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
