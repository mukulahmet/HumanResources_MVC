using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Phone number is required.");
        }

        string phoneNumber = value.ToString();


        if (phoneNumber.Length != 10 || !Regex.IsMatch(phoneNumber, @"^\d{10}$"))
        {
            return new ValidationResult("Phone number must be 10 digits and contain only numbers.");
        }

        return ValidationResult.Success;
    }
}