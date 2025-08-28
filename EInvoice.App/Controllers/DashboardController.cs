using EInvoice.Common.ViewModel;
using EInvoice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EInvoice.App.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {

            return View();
        }
    }
}
