using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class SurnameValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Surname is required.");
        }

        string surname = value.ToString();
  
        if (surname.Length <= 1)
        {
            return new ValidationResult("Surname must be longer than 1 character.");
        }


        if (Regex.IsMatch(surname, @"[^a-zA-Z]"))
        {
            return new ValidationResult("Surname cannot contain numbers or special characters.");
        }

        return ValidationResult.Success;
    }
}