using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Expense;
using HumanResourcesMVC.Models.ViewModels;
using HumanResourcesMVC.Models.ViewModels.Expense;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HumanResourcesMVC.Areas.DirectorPanel.Controllers
{
    [Area("DirectorPanel")]
    public class ExpenseController : Controller
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
        public async Task<IActionResult> ListExpense()
        {
            var userID = HttpContext.Session.GetInt32("userID");
            var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userID}");

            var expenses = await _client.GetFromJsonAsync<List<ListExpenseDirectorVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Expense/SirketeAitExpenseler/{user.CompanyID}");
            return View(expenses);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateApprovalStatus(int expenseId, int newStatus)
        {
            var expense = await _client.GetFromJsonAsync<ListExpenseDirectorIntVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Expense/ExpenseBul/{expenseId}");
            if (expense != null)
            {
                
                UpdateExpenseDTO expenseDTO = new UpdateExpenseDTO();
                expenseDTO.ApprovalStatus= newStatus;
                expenseDTO.Amount = expense.Amount;
                expenseDTO.Currency = expense.Currency;
                expenseDTO.FilePath = expense.FilePath;


                var response = await _client.PutAsJsonAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/Expense/ExpenseGuncelle/{expenseId}", expenseDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Action completed successfully.";
                    return RedirectToAction("ListExpense");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occured.";
                    return RedirectToAction("ListExpense");
                }
            }
            return View("Error");
        }   
    }
}
