namespace HumanResourcesMVC.Areas.EmployeePanel.Models.User
{
    public class AppUserSumVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
		public string? SecondName { get; set; }
		public string LastName { get; set; }
		public string? SecondLastName { get; set; }
		public IFormFile? ImagePath { get; set; }
		public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int JobID { get; set; }
		public int DepartmentID { get; set; }


	}
}
