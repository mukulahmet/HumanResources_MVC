using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class SecondNameValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        string secondName = value.ToString();

        if (Regex.IsMatch(secondName, @"[^a-zA-Z]"))
        {
            return new ValidationResult("Second name cannot contain numbers or special characters.");
        }

        return ValidationResult.Success;
    }
}