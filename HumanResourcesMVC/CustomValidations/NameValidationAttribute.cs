using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class NameValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Name is required.");
        }

        string name = value.ToString();

        if (name.Length <= 1)
        {
            return new ValidationResult("Name must be longer than 1 character.");
        }

        if (Regex.IsMatch(name, @"[^a-zA-Z]"))
        {
            return new ValidationResult("Name cannot contain numbers or special characters.");
        }

        return ValidationResult.Success;
    }
}