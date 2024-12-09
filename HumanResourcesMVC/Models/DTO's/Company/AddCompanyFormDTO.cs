using HumanResourcesMVC.CustomValidations;

namespace HumanResourcesMVC.Models.DTO_s.Company
{
	public class AddCompanyFormDTO
	{
		[CompanyNameValidation]
		public string CompanyName { get; set; }

        [CompanyNameValidation]
        public string CompanyTitle { get; set; }

		[MersisNoValidation]
		public string MersisNo { get; set; }

        [TaxNoValidation]
        public string TaxNo { get; set; }
		public string TaxOffice { get; set; }
		public IFormFile? Logo { get; set; }

		[PhoneNumberValidation]
		public string PhoneNumber { get; set; }
		public string Address { get; set; }

        [EmailValidation]
        public string Email { get; set; }
		public int? EmployeeCount { get; set; }
		public DateTime FoundationDate { get; set; }

        [DateComparisonCompany("ContractExpirationDate")]
        public DateTime? ContractStartDate { get; set; }
		public DateTime? ContractExpirationDate { get; set; }

		public IEnumerable<int>? ListDepartmentID { get; set; }
	}
}
