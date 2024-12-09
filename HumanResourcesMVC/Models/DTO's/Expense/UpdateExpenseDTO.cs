namespace HumanResourcesMVC.Models.DTO_s.Expense
{
    public class UpdateExpenseDTO
    {
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string? FilePath { get; set; }

        public int ApprovalStatus { get; set; }
    }
}
