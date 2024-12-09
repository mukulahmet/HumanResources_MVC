using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HumanResourcesMVC.CustomValidations
{
    public class CompanyNameValidationAttribute : AgeValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Company Name is required.");
            }

            string name = value.ToString();

            if (Regex.IsMatch(name, @"[^a-zA-Z\s]"))
            {
                return new ValidationResult("Name cannot contain numbers or special characters.");
            }

            return ValidationResult.Success;
        }
    }
}
