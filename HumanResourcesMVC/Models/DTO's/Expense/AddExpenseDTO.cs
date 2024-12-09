namespace HumanResourcesMVC.Models.DTO_s.Expense
{
    public class AddExpenseDTO
    {
        public int ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string? FilePath { get; set; }
        public int AppUserID { get; set; }
    }
}
