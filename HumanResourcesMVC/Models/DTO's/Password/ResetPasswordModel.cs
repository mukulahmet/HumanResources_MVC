using System.ComponentModel.DataAnnotations;

namespace HumanResourcesMVC.Models.DTO_s.Password
{
	public class ResetPasswordModel
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string NewPassword { get; set; }

		[Compare("NewPassword",ErrorMessage ="Passwords Does Not Match")]
		public string ConfirmPassword { get; set; }
	}
}
