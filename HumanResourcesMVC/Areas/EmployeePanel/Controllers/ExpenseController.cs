using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Expense;
using HumanResourcesMVC.Models.ViewModels.Expense;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HumanResourcesMVC.Areas.EmployeePanel.Controllers
{
	[Area("EmployeePanel")]
	public class ExpenseController : Controller
	{
        HttpClient _client = new HttpClient();
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
        public async Task<IActionResult> Index()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> AddExpense()
		{
			AddExpenseVM addExpenseVm = new AddExpenseVM();
			return View(addExpenseVm);
		}

		[HttpPost]
		public async Task<IActionResult> AddExpense(AddExpenseVM addExpenseVm)
		{
            var id = HttpContext.Session.GetInt32("userID");

            AddExpenseDTO addExpenseDTO = new AddExpenseDTO();
			addExpenseDTO.Amount = addExpenseVm.Amount;
            addExpenseDTO.Currency = addExpenseVm.Currency;
            addExpenseDTO.ExpenseType = addExpenseVm.ExpenseType;


            if (addExpenseVm.FilePath != null && addExpenseVm.FilePath.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(addExpenseVm.FilePath.FileName)}";
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await addExpenseVm.FilePath.CopyToAsync(stream);
                }

                addExpenseDTO.FilePath = $"/images/{fileName}";
            }


			addExpenseDTO.AppUserID = (int)id;

            var response = await _client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Expense/ExpenseEkle", addExpenseDTO);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "The expense has been successfully created.";
                return RedirectToAction("ListExpense");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occured while creating the expense.";
                return RedirectToAction("ListExpense");
            }

            return RedirectToAction("ListExpense", "Expense", "EmployeePanel");
		}

		[HttpGet]
        public async Task<IActionResult> ListExpense()
        {
            var expenses = await _client.GetFromJsonAsync<List<ListExpenseVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Expense/ExpenseListele");
            expenses = expenses.Where(x => x.AppUserID == HttpContext.Session.GetInt32("userID")).ToList();


            return View(expenses);
        }

    }
}
