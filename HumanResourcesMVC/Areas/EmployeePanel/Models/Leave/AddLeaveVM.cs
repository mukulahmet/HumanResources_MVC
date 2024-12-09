using HumanResourcesMVC.CustomValidations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanResourcesMVC.Areas.EmployeePanel.Models.Leave
{
	public class AddLeaveVM
	{
		public int LeaveType { get; set; }

		// 01.01.0001 görünümünün giderilmesi için prop'un nullable olması ya da default değeri olması lazım. Bu yüzden DateTime.Now default değeri atandı.
		[DateComparisonCompany("LeaveEndDate")]
		public DateTime LeaveStartDate { get; set; } = DateTime.Now;
        public DateTime LeaveEndDate { get; set; } = DateTime.Now;

    }
}
