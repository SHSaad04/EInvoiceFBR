using EInvoice.Common.Entities;
using EInvoice.Domain.Entities;
using EInvoice.Service.Aggregates;
using EInvoice.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.App.Controllers
{
    [Authorize(Roles = UserRoles.OrganizationAdmin)]
    [Route("[controller]")]
    public class ProductController(IProductService productService) : Controller
    {
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await productService.GetAll());
        }
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var product = await productService.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpGet("Upsert/{id?}")]
        public async Task<IActionResult> Upsert(long? id)
        {
            if (id.HasValue)
            {
                var product = await productService.GetById(id.Value);
                if (product == null)
                    return NotFound();
                return View(product);
            }
            return View(new ProductDTO());
        }

        [HttpPost("Upsert/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await productService.Add(model);
            else
                await productService.Edit(model);

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await productService.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await productService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await productService.GetById(id);
            if (product == null)
                return NotFound();

            return Json(new
            {
                description = product.Description,
                hsCode = product.HsCode,
                uom = product.UoM,
                rate = product.Price,
                taxRate = product.TaxRate
            });
        }

    }
}
