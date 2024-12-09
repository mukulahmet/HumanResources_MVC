namespace HumanResourcesMVC.Areas.EmployeePanel.Models.Leave
{
	public class AddLeaveDTO
	{
		public int LeaveType { get; set; }
		public DateTime LeaveStartDate { get; set; }
		public DateTime LeaveEndDate { get; set; }
		public int ApprovalStatus { get; set; }

		public int AppUserID { get; set; }

    }
}
