using System;
using System.ComponentModel.DataAnnotations;

public class AgeValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Birthdate is required.");
        }

        if (!(value is DateTime birthdate))
        {
            return new ValidationResult("Invalid date format.");
        }

        
        DateTime today = DateTime.Today;

        
        int age = today.Year - birthdate.Year;

       
        if (birthdate.Date > today.AddYears(-age))
        {
            age--;
        }

       
        if (age < 18)
        {
            return new ValidationResult("Must be at least 18 years old.");
        }

        return ValidationResult.Success;
    }
}