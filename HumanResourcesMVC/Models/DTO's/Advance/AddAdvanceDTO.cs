namespace HumanResourcesMVC.Models.DTO_s.Advance
{
    public class AddAdvanceDTO
    {
        public int ApprovalStatus { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string Description { get; set; }
        public int AdvanceType { get; set; }

        public int AppUserID { get; set; }
    }
}
