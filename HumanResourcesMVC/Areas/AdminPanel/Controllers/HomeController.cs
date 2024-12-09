using HumanResourcesMVC.Areas.AdminPanel.Models.AppUser;
using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.ViewModels.User.Admin;
using HumanResourcesMVC.Models.ViewModels.User.AllUsers;
using HumanResourcesMVC.Models.ViewModels.User.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Json;

namespace HumanResourcesMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {
        HttpClient _client = new HttpClient();

        // Aksiyon öncesi role dayalı kontrol
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = HttpContext.Session.GetString("userRole");
			// Admin değilse yetkisiz erişim verme
			if (userRole != "Admin")
            {			
				HttpContext.Session.Clear();

				TempData["AlertMessage"] = "Bu sayfaya erisim yetkiniz yok. Lutfen tekrar giris yapiniz";

                context.Result = RedirectToAction("Index", "Home", new { area = "" });
            }

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            // Session'dan ID getirme
            var id = HttpContext.Session.GetInt32("userID");

            var user = await _client.GetFromJsonAsync<IEnumerable<IndexEmployeeVM>>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBulJobVeMeslek/{id}");

            var _user = user.First();

			// Image path kontrolü
			var imagePath = _user.ImagePath;
			if (string.IsNullOrEmpty(imagePath))
			{
				// Eğer resim yolu null veya dosya yoksa varsayılan resmi atar
				_user.ImagePath = "/images/default.jpg";
			}

			AdminIndexVM adminIndexVM = new AdminIndexVM();

            adminIndexVM.User = _user;
            adminIndexVM.DirectorCount = await _client.GetFromJsonAsync<int>("https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/SirketYoneticisiSayisi");
            adminIndexVM.EmployeeCount = await _client.GetFromJsonAsync<int>("https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/PersonelSayisi");
            adminIndexVM.CompanyCount = await _client.GetFromJsonAsync<int>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketSayisi");

			return View(adminIndexVM);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var user = await _client.GetFromJsonAsync<UserDetailVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciDetayBilgileri/{id}");

			var imagePath = user.ImagePath;
			if (string.IsNullOrEmpty(imagePath))
			{
				// Eğer resim yolu null veya dosya yoksa varsayılan resmi atar
				user.ImagePath = "/images/default.jpg";
			}

			return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _client.GetFromJsonAsync<User>($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciBul/{id}");

            AppUserUpdateVM updateVM = new AppUserUpdateVM
            {
                Id = user.Id,
                Name = user.Name,
                SecondName = user.SecondName,
                LastName = user.LastName,
                SecondLastName = user.SecondLastName,
                TC = user.TC,
                Salary = user.Salary,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ImagePath = user.ImagePath,
                ActivityStatus = user.ActivityStatus
            };

            return View(updateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUserUpdateVM user)
        {
            // Model doğrulama kontrolü
            if (!ModelState.IsValid)
            {
                // Model geçerli değilse, hata mesajları ile sayfayı tekrar göster
                return View(user);
            }

            if (user.Photo != null && user.Photo.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(user.Photo.FileName)}";
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await user.Photo.CopyToAsync(stream);
                }

                user.ImagePath = $"/images/{fileName}";
            }

            AppUserUpdateDTO appUserUpdateDTO = new AppUserUpdateDTO
            {
                Id = user.Id,
                Name = user.Name,
                SecondName = user.SecondName,
                LastName = user.LastName,
                SecondLastName = user.SecondLastName,
                TC = user.TC,
                Salary = user.Salary,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ImagePath = user.ImagePath
            };

            var response = await _client.PutAsJsonAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/AppUser/KullaniciGuncelle/{appUserUpdateDTO.Id}", appUserUpdateDTO);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Detail", new { id = user.Id });
            }
            else
            {
                ModelState.AddModelError("", "Failed to update user information.");
                return View(user);
            }
        }
    }
}