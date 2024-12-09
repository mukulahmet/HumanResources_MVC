using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Leave;
using HumanResourcesMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HumanResourcesMVC.Areas.DirectorPanel.Controllers
{
    [Area("DirectorPanel")]
    public class LeaveController : Controller
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
        public async Task<IActionResult> ListLeave()
        {
            var userID = HttpContext.Session.GetInt32("userID");
            var user =await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userID}");   
           
            var leaves = await _client.GetFromJsonAsync<List<ListLeaveDirectorVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/Leave/SirketeAitIzinler/{user.CompanyID}");
           

            return View(leaves);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApprovalStatus(int leaveId, int newStatus)
        {
            var leave = await _client.GetFromJsonAsync<ListLeaveDirectorVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Leave/LeaveBul/{leaveId}");
            if (leave != null)
            {
                leave.ApprovalStatus = newStatus;
                UpdateLeaveDTO leaveDTO = new UpdateLeaveDTO();

                leaveDTO.ApprovalStatus = leave.ApprovalStatus;
                leaveDTO.LeaveEndDate = leave.LeaveEndDate;
                leaveDTO.LeaveStartDate = leave.LeaveStartDate;
                leaveDTO.LeaveType = leave.LeaveType;


                var response = await _client.PutAsJsonAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/Leave/LeaveGuncelle/{leaveId}", leaveDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Action completed successfully.";
                    return RedirectToAction("ListLeave");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occured.";
                    return RedirectToAction("ListLeave");
                }
            }
            return View("Error");
        }


    }
}
