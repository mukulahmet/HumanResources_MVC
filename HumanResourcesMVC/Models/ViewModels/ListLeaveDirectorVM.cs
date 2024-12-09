namespace HumanResourcesMVC.Models.ViewModels
{
    public class ListLeaveDirectorVM
    {
        public int LeaveID { get; set; }
        public int LeaveType { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public int ApprovalStatus { get; set; }
        public int DayCount { get; set; }

        public string FirstName { get; set; }
        public string SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
