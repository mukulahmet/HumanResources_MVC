using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Password;
using HumanResourcesMVC.Models.DTO_s.User.Director;
using HumanResourcesMVC.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace HumanResourcesMVC.Controllers
{
	public class AccountController : Controller
	{
		HttpClient _client = new HttpClient();

		private readonly UserService _userService;
		private readonly IEmailSender _emailSender;

		public AccountController(UserService userService, IEmailSender emailSender)
		{
			_userService = userService;
			_emailSender = emailSender;
		}

		public IActionResult SetPassword(string token, string email)
		{
			return View(new ResetPasswordModel { Token = token, Email = email });
		}

		[HttpPost]
		public async Task<IActionResult> SetPassword(ResetPasswordModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// Şifre belirleme işlemi WebAPI üzerinden gerçekleştirilebilir
			var response = await _client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Account/SetPassword", model);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Home", new { area = "" });
			}

			var errors = await response.Content.ReadAsStringAsync();
			ModelState.AddModelError("", errors);

			return View(model);
		}

		public async Task<IActionResult> ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(string email)
		{

			string serviceEmail = email.Replace("@", "%40");

			try
			{
				var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/MailGirisiKullaniciBul/{serviceEmail}");
			}
			catch (Exception ex)
			{
				TempData["FalseEmail"] = "Please Enter A Valid E-Mail!";
				return RedirectToAction("ForgetPassword", "Account");
			}


			TempData["email"] = $"Mail Sent To {email} Succesfully";

			var token = await _userService.GetPasswordSetTokenAsync(email);
			var resetLink = Url.Action("SetPassword", "Account", new { token, email, area = "" }, protocol: Request.Scheme);

			var message = $"Please set your password by clicking here: <a href=\"{resetLink}\">Set Password</a>";
			await _emailSender.SendEmailAsync(email, "Set Your Password", message);


			return RedirectToAction("Index", "Login");
		}
	}
}
