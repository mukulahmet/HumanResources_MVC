namespace HumanResourcesMVC.Models.ViewModels.Expense
{
	public class AddExpenseVM
	{
		public int ExpenseType { get; set; }
		public decimal Amount { get; set; }
		public int Currency { get; set; }
		public IFormFile? FilePath { get; set; }
	}
}
