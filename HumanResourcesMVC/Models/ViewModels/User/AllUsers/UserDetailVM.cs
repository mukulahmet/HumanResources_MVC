namespace HumanResourcesMVC.Models.ViewModels.User.AllUsers
{
	public class UserDetailVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? SecondName { get; set; }
		public string LastName { get; set; }
		public string? SecondLastName { get; set; }
		public DateTime BirthDate { get; set; }
		public string? BirthPlace { get; set; }
		public string TC { get; set; }
		public string? ImagePath { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? TerminationDate { get; set; }
		public string ActivityStatus { get; set; }
		public string Address { get; set; }
		public decimal? Salary { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public string JobName { get; set; }
		public string DepartmentName { get; set; }
	}
}
