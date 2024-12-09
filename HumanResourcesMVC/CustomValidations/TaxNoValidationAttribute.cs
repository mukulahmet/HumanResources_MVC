using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class TaxNoValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Tax number is required.");
        }

        string taxNo = value.ToString();

        // Vergi numarası 10 haneli ve sadece sayılardan oluşmalı
        if (taxNo.Length != 10 || !taxNo.All(char.IsDigit))
        {
            return new ValidationResult("Tax number must be 10 digits long and contain only numbers.");
        }

        return ValidationResult.Success;
    }
}