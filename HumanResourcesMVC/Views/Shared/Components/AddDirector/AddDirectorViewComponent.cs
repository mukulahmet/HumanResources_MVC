using HumanResourcesMVC.Models.ViewModels.Company;
using HumanResourcesMVC.Models.ViewModels.User.Director;
using HumanResourcesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanResourcesMVC.Views.Shared.Components.AddDirector
{
	public class AddDirectorViewComponent : ViewComponent
	{
		private readonly HttpClient _client;

		public AddDirectorViewComponent(HttpClient client)
		{
			_client = client;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
			var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
			var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

			var vm = new AddDirectorVM
			{
				Company = new SelectList(companies, "CompanyID", "CompanyName"),
				Job = new SelectList(jobs, "JobID", "JobName"),
				Department = new SelectList(departments, "DepartmentID", "DepartmentName")
			};

			return View("Default", vm);
		}
	}
}

