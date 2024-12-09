namespace HumanResourcesMVC.Models.ViewModels.Company
{
	public class ListCompanyVM
	{
		public int CompanyID { get; set; }
		public string CompanyName { get; set; }
		public string CompanyTitle { get; set; }
		public string? LogoPath { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
	}
}
