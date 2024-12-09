using HumanResourcesMVC.Models.DTO_s.Company;
using HumanResourcesMVC.Models.DTO_s.Department;

namespace HumanResourcesMVC.Models.ViewModels.Company
{
    public class AddCompanyVM
    {
        public AddCompanyFormDTO AddCompanyFormDTO { get; set; }

        public List<AssignDepartment> SelectedDepartments { get; set; }

    }
}
