namespace HumanResourcesMVC.Models.DTO_s.Company
{
    public class AddCompanyDTO
    {
        public string CompanyName { get; set; }
        public string CompanyTitle { get; set; }
        [MersisNoValidation]
        public string MersisNo { get; set; }
        [TaxNoValidation]
        public string TaxNo { get; set; }
        public string TaxOffice { get; set; }
        public string? LogoPath { get; set; }
        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        public int? EmployeeCount { get; set; }
        public DateTime FoundationDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpirationDate { get; set; }

        public IEnumerable<int> ListDepartmentID { get; set; }
    }
}
