using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HumanResourcesMVC.CustomValidations
{
    public class SecondSurnameValidationAttribute:ValidationAttribute
    {
        override protected ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string secondSurname = value.ToString();

            if (Regex.IsMatch(secondSurname, @"[^a-zA-Z]"))
            {
                return new ValidationResult("Second surname cannot contain numbers or special characters.");
            }

            return ValidationResult.Success;
        }
    }
}
