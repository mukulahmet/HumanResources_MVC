namespace HumanResourcesMVC.Models.DTO_s.Leave
{
    public class UpdateLeaveDTO
    {
        public int LeaveType { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
