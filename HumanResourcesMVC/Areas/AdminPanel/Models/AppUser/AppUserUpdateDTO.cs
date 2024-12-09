namespace HumanResourcesMVC.Areas.AdminPanel.Models.AppUser
{
    public class AppUserUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }

        public string TC { get; set; }
        public string? ImagePath { get; set; }

        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
        public decimal? Salary { get; set; }
    }
}
