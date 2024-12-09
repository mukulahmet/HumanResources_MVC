using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class MersisNoValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("MERSIS number is required.");
        }

        string mersisNo = value.ToString();

        // MERSIS numarası 16 haneli ve sadece sayılardan oluşmalı
        if (mersisNo.Length != 16 || !mersisNo.All(char.IsDigit))
        {
            return new ValidationResult("MERSIS number must be 16 digits long and contain only numbers.");
        }

        return ValidationResult.Success;
    }
}