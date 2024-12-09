using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.ViewModels.Company;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HumanResourcesMVC.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient();
        public async Task<IActionResult> Index()
        {
            var companies = await client.GetFromJsonAsync<IEnumerable<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");

            return View(companies);
        }
    }
}
