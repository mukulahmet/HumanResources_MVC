using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Advance;
using HumanResourcesMVC.Models.ViewModels.Advance;
using HumanResourcesMVC.Models.ViewModels.Company;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.Design;

namespace HumanResourcesMVC.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class AdvanceController : Controller
    {
        HttpClient client = new HttpClient();
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = HttpContext.Session.GetString("userRole");

            if (userRole != "Employee")
            {
                HttpContext.Session.Clear();

                TempData["AlertMessage"] = "Bu sayfaya erisim yetkiniz yok. Lutfen tekrar giris yapiniz";

                context.Result = RedirectToAction("Index", "Home", new { area = "" });
            }

            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {

            // Advance sekmesine tiklayinca 2 tane buttonla karsilasip advance talep et ve advance taleplerimi goruntule diye olacak... 

            return View();
        }

        public async Task<IActionResult> AdvanceList()
        {

            var userID = HttpContext.Session.GetInt32("userID");

            var allAdvances = await client.GetFromJsonAsync<IEnumerable<ListAdvanceVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Advance/KullaniciAdvanceleri/{userID}");

            return View(allAdvances);
        }

        public async Task<IActionResult> NewAdvance()
        {
            var userId = HttpContext.Session.GetInt32("userID");
            var user = await client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userId}");

            // Salary decimal olduğu için önce string'e çevirip oturuma kaydediyoruz
            HttpContext.Session.SetString("salary", user.Salary.GetValueOrDefault(0).ToString());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewAdvance(AddAdvanceVM vm)
        {
            if (ModelState.IsValid)
            {
                AddAdvanceDTO addAdvanceDTO = new AddAdvanceDTO();
                addAdvanceDTO.AdvanceType = vm.AdvanceType;
                addAdvanceDTO.Currency = vm.Currency;
                addAdvanceDTO.Amount = vm.Amount;
                addAdvanceDTO.Description = vm.Description;

                addAdvanceDTO.ApprovalStatus = 2;
                addAdvanceDTO.AppUserID = (int)HttpContext.Session.GetInt32("userID");

                var response = await client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Advance/AdvanceEkle", addAdvanceDTO);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "The advance has been successfully created.";
                    return RedirectToAction("AdvanceList");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occured while creating the advance.";
                    return RedirectToAction("AdvanceList");
                }

                return RedirectToAction("AdvanceList", "Advance");
            }
            return View(vm);
        }


    }
}
