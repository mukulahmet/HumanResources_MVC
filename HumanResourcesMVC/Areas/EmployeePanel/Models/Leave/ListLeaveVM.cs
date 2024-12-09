namespace HumanResourcesMVC.Areas.EmployeePanel.Models.Leave
{
	public class ListLeaveVM
	{
		public int LeaveID { get; set; }
		public int AppUserID { get; set; }
		public int LeaveType { get; set; }
		public DateTime LeaveStartDate { get; set; }
		public DateTime LeaveEndDate { get; set; }
		public int? DayCount { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
