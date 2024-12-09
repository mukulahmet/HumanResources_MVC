using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Advance;
using HumanResourcesMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HumanResourcesMVC.Areas.DirectorPanel.Controllers
{
    [Area("DirectorPanel")]
    public class AdvanceController : Controller
    {
        HttpClient _client = new HttpClient();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = HttpContext.Session.GetString("userRole");

            if (userRole != "Director")
            {
                HttpContext.Session.Clear();

                TempData["AlertMessage"] = "Bu sayfaya erisim yetkiniz yok. Lutfen tekrar giris yapiniz";

                context.Result = RedirectToAction("Index", "Home", new { area = "" });
            }

            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListAdvance()
        {
            var userID = HttpContext.Session.GetInt32("userID");
            var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userID}");

            var advances = await _client.GetFromJsonAsync<List<ListAdvanceDirectorVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Advance/SirketeAitAdvanceler/{user.CompanyID}");

            return View(advances);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApprovalStatus(int advanceId, int newStatus)
        {
            var advance = await _client.GetFromJsonAsync<ListAdvanceDirectorIntVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Advance/AdvanceGetir/{advanceId}");
            if (advance != null)
            {
            
                UpdateAdvanceDTO advanceDTO = new UpdateAdvanceDTO();

                advanceDTO.ApprovalStatus = newStatus;
                advanceDTO.Amount = advance.Amount;
                advanceDTO.Currency = advance.Currency;
                advanceDTO.Description = advance.Description;
                advanceDTO.AdvanceType = advance.AdvanceType;

                var response = await _client.PutAsJsonAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/Advance/AdvanceGuncelle/{advanceId}", advanceDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Action completed successfully.";
                    return RedirectToAction("ListAdvance");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occured.";
                    return RedirectToAction("ListAdvance");
                }
            }
            return View("Error");
        }
    }
}
