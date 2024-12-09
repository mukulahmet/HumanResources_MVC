using HumanResourcesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace HumanResourcesMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<string> GetTokenAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Login/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                throw new Exception("Giriş başarısız");
            }
        }

        private string DecodeTokenAndGetUserId(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Geçersiz JWT token formatı");
            }

            var payload = parts[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var jsonString = Encoding.UTF8.GetString(jsonBytes);
            var jsonObject = JObject.Parse(jsonString);

            return jsonObject["UserId"]?.ToString();
        }

        private string DecodeTokenAndGetUserRole(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Geçersiz JWT token formatı");
            }

            var payload = parts[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var jsonString = Encoding.UTF8.GetString(jsonBytes);
            var jsonObject = JObject.Parse(jsonString);

            return jsonObject[ClaimTypes.Role]?.ToString();
        }

        private string PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await GetTokenAsync(model.Email, model.Password);
                    HttpContext.Session.SetString("AuthToken", token);

                    // Kullanıcı Id'sini token'dan çıkar
                    var userId = DecodeTokenAndGetUserId(token);

                    //ID Session'a eklendi
                    HttpContext.Session.SetInt32("userID", int.Parse(userId));
                    


                    // Kullanıcı rolünü token'dan çıkar
                    var userRole = DecodeTokenAndGetUserRole(token);

                    // Rol bilgisi Session'a eklendi
                    HttpContext.Session.SetString("userRole", userRole);

                    if (userRole == "Admin")
                    {
                        return RedirectToAction("Index", "Home", new { area = "AdminPanel" });
                    }
                    else if (userRole == "Director")
                    {
                        return RedirectToAction("Index", "Home", new { area = "DirectorPanel" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "EmployeePanel" });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View("Index", model);
        }

        public async Task<IActionResult> Logout()
        {
			HttpContext.Session.Clear();

			

            var result = _httpClient.GetFromJsonAsync<bool>("https://projectwebapi220240824154733.azurewebsites.net/api/Login/CikisYap");

			// Ana sayfaya yönlendir
			return RedirectToAction("Index", "Home");
		}
    }
}