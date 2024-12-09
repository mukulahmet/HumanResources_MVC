namespace HumanResourcesMVC.Models.ViewModels.User.Director
{
    public class ListDirectorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string CompanyName { get; set; }
    }
}
