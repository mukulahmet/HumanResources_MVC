
using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.DTO_s.Company;
using HumanResourcesMVC.Models.DTO_s.Department;
using HumanResourcesMVC.Models.ViewModels.Company;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.WebSockets;

namespace HumanResourcesMVC.Areas.AdminPanel.Controllers
{
	[Area("AdminPanel")]
	public class CompanyController : Controller
	{
		HttpClient client = new HttpClient();

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

		public async Task<IActionResult> ListCompany()
		{
			var allCompanies = await client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");

			return View(allCompanies);
		}

		public async Task<IActionResult> GetCompanyDetail(int id)
		{
			var companyDetail = await client.GetFromJsonAsync<DetailCompanyVM>($"https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketGetir/{id}");

			return View(companyDetail);
		}

		public async Task<IActionResult> DeleteCompany(int id)
		{
			var response = await client.DeleteAsync($"https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketSil/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "The company has been successfully deleted.";
                return RedirectToAction("ListCompany");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occured while deleting the company.";
                return RedirectToAction("ListCompany");
            }
        }

		public async Task<IActionResult> AddCompany()
		{
			AddCompanyVM addCompanyVM = new AddCompanyVM();

			var allDepartments = await client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

			List<AssignDepartment> assignDepartments = new List<AssignDepartment>();

            foreach (var department in allDepartments)
            {
				AssignDepartment addDepartment = new AssignDepartment();

				addDepartment.DepartmentID = department.DepartmentID;
				addDepartment.DepartmentName = department.DepartmentName;

				assignDepartments.Add(addDepartment);
            }

			addCompanyVM.SelectedDepartments = assignDepartments;


            return View(addCompanyVM);
		}

        [HttpPost]
		public async Task<IActionResult> AddCompany(AddCompanyVM addCompanyVM)
		{
            if (!ModelState.IsValid)
            {
                return View(addCompanyVM);
            }


            AddCompanyDTO addCompanyDTO = new AddCompanyDTO();
			addCompanyDTO.CompanyName = addCompanyVM.AddCompanyFormDTO.CompanyName;
			addCompanyDTO.CompanyTitle = addCompanyVM.AddCompanyFormDTO.CompanyTitle;
			addCompanyDTO.MersisNo = addCompanyVM.AddCompanyFormDTO.MersisNo;
			addCompanyDTO.TaxNo = addCompanyVM.AddCompanyFormDTO.TaxNo;
			addCompanyDTO.TaxOffice = addCompanyVM.AddCompanyFormDTO.TaxOffice;

			//VM tarafının içersindeki IFormFile olmalı onunu için de VM içindeki model degismeli...

			if (addCompanyVM.AddCompanyFormDTO.Logo != null && addCompanyVM.AddCompanyFormDTO.Logo.Length > 0)
			{
				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(addCompanyVM.AddCompanyFormDTO.Logo.FileName)}";
				var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

				using (var stream = new FileStream(savePath, FileMode.Create))
				{
					await addCompanyVM.AddCompanyFormDTO.Logo.CopyToAsync(stream);
				}

				addCompanyDTO.LogoPath = $"/images/{fileName}";
			}
			else
			{
                addCompanyDTO.LogoPath = $"/images/default_company.jpg";
            }


			addCompanyDTO.PhoneNumber = addCompanyVM.AddCompanyFormDTO.PhoneNumber;
			addCompanyDTO.Address = addCompanyVM.AddCompanyFormDTO.Address;
			addCompanyDTO.Email = addCompanyVM.AddCompanyFormDTO.Email;
			addCompanyDTO.EmployeeCount = addCompanyVM.AddCompanyFormDTO.EmployeeCount;
			addCompanyDTO.FoundationDate = addCompanyVM.AddCompanyFormDTO.FoundationDate;
			addCompanyDTO.ContractStartDate = addCompanyVM.AddCompanyFormDTO.ContractStartDate;
			addCompanyDTO.ContractExpirationDate = addCompanyVM.AddCompanyFormDTO.ContractExpirationDate;

			List<int> selectedDepartmentIDs = new List<int>();

            foreach (var department in addCompanyVM.SelectedDepartments)
            {
				if (department.IsSelected)
				{
					selectedDepartmentIDs.Add(department.DepartmentID);
				}
            }

			if (selectedDepartmentIDs.Count==0)
			{
				TempData["AlertDepartment"] = "Please Choose minimum 1 Department";
                return View(addCompanyVM);
			}

			addCompanyDTO.ListDepartmentID = selectedDepartmentIDs;

			var addCompany = await client.PostAsJsonAsync("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketEkle", addCompanyDTO);

			if (addCompany.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "The company has been successfully created.";
				return RedirectToAction("ListCompany");
            }
			else
			{
                TempData["ErrorMessage"] = "An error occured while creating the company.";
                return RedirectToAction("ListCompany");
            }
			
            return RedirectToAction("Index","Home");

        }


	}
}
