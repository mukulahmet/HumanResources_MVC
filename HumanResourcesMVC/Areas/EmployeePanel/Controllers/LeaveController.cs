using HumanResourcesMVC.Areas.EmployeePanel.Models.Leave;
using HumanResourcesMVC.Models.ViewModels.Company;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HumanResourcesMVC.Areas.EmployeePanel.Controllers
{
	[Area("EmployeePanel")]
	public class LeaveController : Controller
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

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			AddLeaveVM addLeaveVM = new AddLeaveVM();

			return View(addLeaveVM);
		}

		[HttpPost]
		public async Task<IActionResult> Index(AddLeaveVM addLeaveVM)
		{
            if (!ModelState.IsValid)
			{
				return View(addLeaveVM);
			}

            var id = HttpContext.Session.GetInt32("userID");

			AddLeaveDTO addLeaveDTO = new AddLeaveDTO();
			addLeaveDTO.LeaveType = addLeaveVM.LeaveType;
			addLeaveDTO.LeaveStartDate = addLeaveVM.LeaveStartDate;
			addLeaveDTO.LeaveEndDate = addLeaveVM.LeaveEndDate;
			addLeaveDTO.AppUserID = (int)id;
			addLeaveDTO.ApprovalStatus = 2;

			var response = await _client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Leave/LeaveEkle", addLeaveDTO);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "The leave has been successfully created.";
                return RedirectToAction("ListLeave");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occured while creating the leave.";
                return RedirectToAction("ListLeave");
            }

            return RedirectToAction("ListLeave", "Leave", "EmployeePanel");
		}

		[HttpGet]
		public async Task<IActionResult> ListLeave()
		{
			var leaves = await _client.GetFromJsonAsync<List<ListLeaveVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Leave/LeaveListele");
			leaves = leaves.Where(x => x.AppUserID == HttpContext.Session.GetInt32("userID")).ToList();


			return View(leaves);
		}


	}
}
