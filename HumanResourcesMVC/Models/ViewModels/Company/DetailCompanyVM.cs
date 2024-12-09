namespace HumanResourcesMVC.Models.ViewModels.Company
{
	public class DetailCompanyVM
	{
		public string CompanyName { get; set; }
		public string CompanyTitle { get; set; }
		public string MersisNo { get; set; }
		public string TaxNo { get; set; }
		public string TaxOffice { get; set; }
		public string? LogoPath { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public int? EmployeeCount { get; set; }
		public DateTime FoundationDate { get; set; }
		public DateTime? ContractStartDate { get; set; }
		public DateTime? ContractExpirationDate { get; set; }

		public IEnumerable<int> ListDepartmentID { get; set; }
	}
}

