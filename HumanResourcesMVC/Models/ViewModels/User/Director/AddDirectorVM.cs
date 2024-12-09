using HumanResourcesMVC.Models.DTO_s.User.Director;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanResourcesMVC.Models.ViewModels.User.Director
{
	public class AddDirectorVM
	{
        public AddDirectorFormDTO addDirectorFormDTO { get; set; }

        public SelectList? Company { get; set; }
		public SelectList? Job { get; set; }
		public SelectList? Department { get; set; }
	}
}
