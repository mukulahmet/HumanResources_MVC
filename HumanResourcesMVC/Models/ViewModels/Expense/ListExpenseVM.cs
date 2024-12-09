namespace HumanResourcesMVC.Models.ViewModels.Expense
{
    public class ListExpenseVM
    {
        public int ExpenseID { get; set; }
        public int AppUserID { get; set; }
        public int ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string? FilePath { get; set; }
        public int ApprovalStatus { get; set; }

    }
}
