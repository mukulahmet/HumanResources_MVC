using HumanResourcesMVC.Models;
using HumanResourcesMVC.Models.ViewModels.Company;
using HumanResourcesMVC.Models.ViewModels.User.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanResourcesMVC.Views.Shared.Components.AddEmployee
{
    public class AddEmployeeViewComponent : ViewComponent
    {
        private readonly HttpClient _client;

        public AddEmployeeViewComponent(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AddEmployeeVM vm = new AddEmployeeVM();

            var companies = await _client.GetFromJsonAsync<List<ListCompanyVM>>("https://projectwebapi220240824154733.azurewebsites.net/api/Company/SirketleriGetir");
            var jobs = await _client.GetFromJsonAsync<List<Job>>("https://projectwebapi220240824154733.azurewebsites.net/api/Job/IsleriGetir");
            var departments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");

            vm.Company = new SelectList(companies, "CompanyID", "CompanyName");
            vm.Department = new SelectList(departments, "DepartmentID", "DepartmentName");
            vm.Job = new SelectList(jobs, "JobID", "JobName");

            return View("Default", vm);
        }
    }
}
