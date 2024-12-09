﻿using System.Diagnostics;
using HumanResourcesMVC.Models.Enums;
using HumanResourcesMVC.CustomValidations;

namespace HumanResourcesMVC.Areas.AdminPanel.Models.AppUser
{
	public class AppUserUpdateVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }

        public string TC { get; set; }
        public string? ImagePath { get; set; }

        [PhoneNumberValidation]
        public string? PhoneNumber { get; set; }

        public string Address { get; set; }
        public decimal? Salary { get; set; }

        public ActivityStatus? ActivityStatus { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
