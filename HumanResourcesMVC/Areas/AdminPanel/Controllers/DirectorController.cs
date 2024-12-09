using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Company;
using HumanResourcesMVC.Models.DTO_s.User.Director;
using HumanResourcesMVC.Models.ViewModels.Company;
using HumanResourcesMVC.Models.ViewModels.User.Director;
using HumanResourcesMVC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel;

namespace HumanResourcesMVC.Areas.AdminPanel.Controllers
{
	[Area("AdminPanel")]
	public class DirectorController : Controller
	{
		HttpClient _client = new HttpClient();

		private readonly UserService _userService;
		private readonly IEmailSender _emailSender;

		public DirectorController(UserService userService, IEmailSender emailSender)
		{
			_userService = userService;
			_emailSender = emailSender;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userRole = HttpContext.Session.GetString("userRole");

			if (userRole != "Admin")
			{
				HttpContext.Session.Clear();

				TempData["AlertMessage"] = "Bu sayfaya erisim yetkiniz yok. Lutfen tekrar giris yapiniz";

				context.Result = RedirectToAction("Index", "Home", new { area = "" });
			}

			base.OnActionExecuting(context);
		}
		public async Task<IActionResult> ListDirector()
		{
			var directors = await _client.GetFromJsonAsync<List<ListDirectorVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/SirketYoneticileriniGetir");

			return View(directors);
		}

		public async Task<IActionResult> DeleteDirector(int id)
		{
			var response = await _client.DeleteAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciSil/{id}");

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "The director has been successfully deleted.";
				return RedirectToAction("ListDirector");
			}
			else
			{
				TempData["ErrorMessage"] = "An error occured while deleting the director.";
				return RedirectToAction("ListDirector");
			}
		}

		public async Task<IActionResult> AddDirector()
		{
			AddDirectorVM vm = new AddDirectorVM();

			var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
			var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
			var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

            var directorJobs = jobs.Where(j => j.JobName == "Director").ToList();

            vm.Company = new SelectList(companies, "CompanyID", "CompanyName");
			vm.Department = new SelectList(departments, "DepartmentID", "DepartmentName");
			vm.Job = new SelectList(directorJobs, "JobID", "JobName");

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> AddDirector(AddDirectorVM addDirectorVM)
		{
			if (ModelState.IsValid)
			{
                var MailCheck = addDirectorVM.addDirectorFormDTO.Email;               

                try
                {
                    var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/MailGirisiKullaniciBul/{MailCheck}");
					if (user != null)
					{
                        TempData["MailControl"] = "This E-Mail is already taken!";
                        return RedirectToAction("ListDirector", "Director");
                    }
                }
                catch (Exception ex)
                {

                }

                AddDirectorDTO addDirectorDTO = new AddDirectorDTO();

				addDirectorDTO.Name = addDirectorVM.addDirectorFormDTO.Name;
				addDirectorDTO.SecondName = addDirectorVM.addDirectorFormDTO.SecondName;
				addDirectorDTO.LastName = addDirectorVM.addDirectorFormDTO.LastName;
				addDirectorDTO.SecondLastName = addDirectorVM.addDirectorFormDTO.SecondLastName;

				// Burası yine company usulu degistirilecek...
				//addDirectorDTO.ImagePath = addDirectorVM.addDirectorDTO.ImagePath;

				if (addDirectorVM.addDirectorFormDTO.Photo != null && addDirectorVM.addDirectorFormDTO.Photo.Length > 0)
				{
					var fileName = $"{Guid.NewGuid()}{Path.GetExtension(addDirectorVM.addDirectorFormDTO.Photo.FileName)}";
					var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

					using (var stream = new FileStream(savePath, FileMode.Create))
					{
						await addDirectorVM.addDirectorFormDTO.Photo.CopyToAsync(stream);
					}

					addDirectorDTO.ImagePath = $"/images/{fileName}";
				}
				else
				{
					addDirectorDTO.ImagePath = $"/images/default.jpg";
				}

				addDirectorDTO.Address = addDirectorVM.addDirectorFormDTO.Address;
				addDirectorDTO.TC = addDirectorVM.addDirectorFormDTO.TC;
				addDirectorDTO.BirthDate = addDirectorVM.addDirectorFormDTO.BirthDate;
				addDirectorDTO.BirthPlace = addDirectorVM.addDirectorFormDTO.BirthPlace;
				addDirectorDTO.StartDate = addDirectorVM.addDirectorFormDTO.StartDate;
				addDirectorDTO.Email = addDirectorVM.addDirectorFormDTO.Email;
				addDirectorDTO.PhoneNumber = addDirectorVM.addDirectorFormDTO.PhoneNumber;
				addDirectorDTO.JobID = addDirectorVM.addDirectorFormDTO.JobID;
				addDirectorDTO.CompanyID = addDirectorVM.addDirectorFormDTO.CompanyID;
				addDirectorDTO.DepartmentID = addDirectorVM.addDirectorFormDTO.DepartmentID;

				var result = await _client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/DirectorEkle", addDirectorDTO);

				if (result.IsSuccessStatusCode)
				{
					var token = await _userService.GetPasswordSetTokenAsync(addDirectorDTO.Email);
					var resetLink = Url.Action("SetPassword", "Account", new { token, addDirectorDTO.Email, area = "" }, protocol: Request.Scheme);

					var message = $"Please set your password by clicking here: <a href=\"{resetLink}\">Set Password</a>";
					await _emailSender.SendEmailAsync(addDirectorDTO.Email, "Set Your Password", message);

                    TempData["SuccessMessage"] = "The director has been successfully created.";
                    return RedirectToAction("ListDirector");
                }
				else
				{
                    TempData["ErrorMessage"] = "An error occured while creating the director.";
                    return RedirectToAction("ListDirector");
                }

			}

            AddDirectorVM vm = new AddDirectorVM();

            var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
            var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
            var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

            var directorJobs = jobs.Where(j => j.JobName == "Director").ToList();

            vm.Company = new SelectList(companies, "CompanyID", "CompanyName");
            vm.Department = new SelectList(departments, "DepartmentID", "DepartmentName");
            vm.Job = new SelectList(directorJobs, "JobID", "JobName");

            return View(vm);
		}
	}
}
