namespace HumanResourcesMVC.Models.ViewModels
{
    public class ListExpenseDirectorIntVM
    {
        public int ExpenseID { get; set; }
        public int ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }

        public int ApprovalStatus { get; set; }
        public string? FilePath { get; set; }

        public string FirstName { get; set; }
        public string SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        public DateTime? CreationDate { get; set; }
       
    }
}
