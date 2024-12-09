namespace HumanResourcesMVC.Models.ViewModels
{
    public class ListExpenseDirectorVM
    {
        public int ExpenseID { get; set; }
        public string ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? FilePath { get; set; }

        public string FirstName { get; set; }
        public string SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
