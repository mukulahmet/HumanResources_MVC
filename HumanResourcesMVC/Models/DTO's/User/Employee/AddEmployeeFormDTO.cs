﻿using HumanResourcesMVC.CustomValidations;
using System.ComponentModel;

namespace HumanResourcesMVC.Models.DTO_s.User.Employee
{
    public class AddEmployeeFormDTO
    {
        [NameValidation]
        public string Name { get; set; }
        [SecondNameValidation]
        public string? SecondName { get; set; }
        [SurnameValidation]
        public string LastName { get; set; }
        [SecondSurnameValidation]
        public string? SecondLastName { get; set; }
        public IFormFile? Photo { get; set; }
        [AgeValidation]
        public DateTime BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public decimal? Salary { get; set; }

        [IdentificationNumberValidation]
        public string TC { get; set; }
        [DateComparison("BirthDate")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Job")]
        public int JobID { get; set; }

        [DisplayName("Department")]
        public int DepartmentID { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        public string Address { get; set; }
        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }

        [DisplayName("Company")]
        public int CompanyID { get; set; }
    }
}
