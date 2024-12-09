using HumanResourcesMVC.Models.DTO_s.User.Employee;
using HumanResourcesMVC.Models.ViewModels.Company;
using HumanResourcesMVC.Models.ViewModels.User.Employee;
using HumanResourcesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using HumanResourcesMVC.Models.DTO_s.User.Director;
using HumanResourcesMVC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using HumanResourcesMVC.Models.ViewModels.User.Director;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net.Http;
using HumanResourcesMVC.Models.DTO_s.Company;
using Microsoft.AspNetCore.Http;

namespace HumanResourcesMVC.Areas.DirectorPanel.Controllers
{
    [Area("DirectorPanel")]
    public class EmployeeController : Controller
    {
        HttpClient _client = new HttpClient();

        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;

        public EmployeeController(UserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

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

        public async Task<IActionResult> AddEmployee()
        {

            AddEmployeeVM vm = new AddEmployeeVM();

            var userId = HttpContext.Session.GetInt32("userID");
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            // Kullanıcı bilgilerini almak için API çağrısı yap
            var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userId}");
            var companyId = user.CompanyID;
            var company = await _client.GetFromJsonAsync<DetailCompanyVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketGetir/{companyId}");
            var companyName = company.CompanyName;

            HttpContext.Session.SetInt32("companyId", companyId.GetValueOrDefault(0));
            HttpContext.Session.SetString("companyName", companyName);

            var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
            var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
            var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

            var employeeJobs = jobs.Where(j => j.JobName != "Director").ToList();

            vm.Company = new SelectList(companies, "CompanyID", "CompanyName");
            vm.Department = new SelectList(departments, "DepartmentID", "DepartmentName");
            vm.Job = new SelectList(employeeJobs, "JobID", "JobName");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeVM addEmployeeVM)
        {
            foreach (var key in ModelState.Keys)
            {
                var value = ModelState[key];
                if (value.Errors.Count > 0)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                AddEmployeeVM vm = new AddEmployeeVM();

                var userId = HttpContext.Session.GetInt32("userID");
                if (!userId.HasValue)
                {
                    return Unauthorized();
                }

                // Kullanıcı bilgilerini almak için API çağrısı yap
                var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{userId}");
                var companyId = user.CompanyID;
                var company = await _client.GetFromJsonAsync<DetailCompanyVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketGetir/{companyId}");
                var companyName = company.CompanyName;

                var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
                var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
                var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

                var employeeJobs = jobs.Where(j => j.JobName != "Director").ToList();

                vm.Company = new SelectList(companies, "CompanyID", "CompanyName");
                vm.Department = new SelectList(departments, "DepartmentID", "DepartmentName");
                vm.Job = new SelectList(employeeJobs, "JobID", "JobName");

                return View(vm);
            }

            var MailCheck = addEmployeeVM.addEmployeeFormDTO.Email;

            try
            {
                var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/MailGirisiKullaniciBul/{MailCheck}");
                if (user != null)
                {
                    TempData["MailControl2"] = "This E-Mail is already taken!";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

            }

            AddEmployeeDTO addEmployeeDTO = new AddEmployeeDTO();

            addEmployeeDTO.Name = addEmployeeVM.addEmployeeFormDTO.Name;
            addEmployeeDTO.SecondName = addEmployeeVM.addEmployeeFormDTO.SecondName;
            addEmployeeDTO.LastName = addEmployeeVM.addEmployeeFormDTO.LastName;
            addEmployeeDTO.SecondLastName = addEmployeeVM.addEmployeeFormDTO.SecondLastName;

            // Burası yine company usulu degistirilecek...
            //addEmployeeDTO.ImagePath = addEmployeeVM.addEmployeeDTO.ImagePath;

            if (addEmployeeVM.addEmployeeFormDTO.Photo != null && addEmployeeVM.addEmployeeFormDTO.Photo.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(addEmployeeVM.addEmployeeFormDTO.Photo.FileName)}";
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await addEmployeeVM.addEmployeeFormDTO.Photo.CopyToAsync(stream);
                }

                addEmployeeDTO.ImagePath = $"/images/{fileName}";
            }
            else
            {
                addEmployeeDTO.ImagePath = $"/images/default.jpg";
            }

            addEmployeeDTO.Address = addEmployeeVM.addEmployeeFormDTO.Address;
            addEmployeeDTO.TC = addEmployeeVM.addEmployeeFormDTO.TC;
            addEmployeeDTO.BirthDate = addEmployeeVM.addEmployeeFormDTO.BirthDate;
            addEmployeeDTO.BirthPlace = addEmployeeVM.addEmployeeFormDTO.BirthPlace;
            addEmployeeDTO.StartDate = addEmployeeVM.addEmployeeFormDTO.StartDate;
            addEmployeeDTO.Salary = addEmployeeVM.addEmployeeFormDTO.Salary;
            addEmployeeDTO.Email = addEmployeeVM.addEmployeeFormDTO.Email;
            addEmployeeDTO.PhoneNumber = addEmployeeVM.addEmployeeFormDTO.PhoneNumber;
            addEmployeeDTO.JobID = addEmployeeVM.addEmployeeFormDTO.JobID;
            addEmployeeDTO.CompanyID = addEmployeeVM.addEmployeeFormDTO.CompanyID;
            addEmployeeDTO.DepartmentID = addEmployeeVM.addEmployeeFormDTO.DepartmentID;

            var result = await _client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/EmployeeEkle", addEmployeeDTO);

            if (result.IsSuccessStatusCode)
            {
                var token = await _userService.GetPasswordSetTokenAsync(addEmployeeDTO.Email);
                var resetLink = Url.Action("SetPassword", "Account", new { token, addEmployeeDTO.Email, area = "" }, protocol: Request.Scheme);

                var message = $"Please set your password by clicking here: <a href=\"{resetLink}\">Set Password</a>";
                await _emailSender.SendEmailAsync(addEmployeeDTO.Email, "Set Your Password", message);

                TempData["SuccessMessage"] = "The employee has been successfully created.";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occured while creating the employee.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
