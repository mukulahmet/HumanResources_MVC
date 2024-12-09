namespace HumanResourcesMVC.Models.ViewModels
{
    public class ListAdvanceDirectorVM
    {
        public int AdvanceID { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string AdvanceType { get; set; }

        public string FirstName { get; set; }
        public string SecondFirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        public DateTime? CreationDate { get; set; } //aynı zamanda talep tarihi 
    }
}
