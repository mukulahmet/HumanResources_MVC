using HumanResourcesMVC.Models.ViewModels.User.Employee;

namespace HumanResourcesMVC.Models.ViewModels.User.Admin
{
	public class AdminIndexVM
	{
        public IndexEmployeeVM User { get; set; }
        public int CompanyCount { get; set; }
        public int DirectorCount { get; set; }
        public int EmployeeCount { get; set; }
    }
}
