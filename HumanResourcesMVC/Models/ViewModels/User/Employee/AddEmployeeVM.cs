using HumanResourcesMVC.Models.DTO_s.Company;
using HumanResourcesMVC.Models.DTO_s.User.Employee;
using HumanResourcesMVC.Models.ViewModels.Company;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanResourcesMVC.Models.ViewModels.User.Employee
{
    public class AddEmployeeVM
    {
        public AddEmployeeFormDTO addEmployeeFormDTO { get; set; }
        public SelectList? Company { get; set; }
        public SelectList? Job { get; set; }
        public SelectList? Department { get; set; }
    }
}
