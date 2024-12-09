using HumanResourcesMVC.Models.ViewModels.Company;
using HumanResourcesMVC.Models.ViewModels.User.Director;
using HumanResourcesMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HumanResourcesMVC.Models.DTO_s.Department;

namespace HumanResourcesMVC.Views.Shared.Components.AddCompany
{
    public class AddCompanyViewComponent : ViewComponent
    {
        private readonly HttpClient _client;

        public AddCompanyViewComponent(HttpClient client)
        {
            _client = client;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AddCompanyVM addCompanyVM = new AddCompanyVM();
            var allDepartments = await _client.GetFromJsonAsync<List<Department>>("https://projectwebapi220240824154733.azurewebsites.net/api/Department/GetDepartments");
            List<AssignDepartment> assignDepartments = new List<AssignDepartment>();

            foreach (var department in allDepartments)
            {
                AssignDepartment addDepartment = new AssignDepartment();

                addDepartment.DepartmentID = department.DepartmentID;
                addDepartment.DepartmentName = department.DepartmentName;

                assignDepartments.Add(addDepartment);
            }

            addCompanyVM.SelectedDepartments = assignDepartments;

            return View("Default", addCompanyVM);
        }
    }
}
