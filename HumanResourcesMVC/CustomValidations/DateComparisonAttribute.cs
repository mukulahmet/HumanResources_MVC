using System;
using System.ComponentModel.DataAnnotations;

public class DateComparisonAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateComparisonAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Start date is required.");
        }

        var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (comparisonProperty == null)
        {
            return new ValidationResult($"Unknown property: {_comparisonProperty}");
        }

        var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);

        if (comparisonValue == null)
        {
            return new ValidationResult("Birthdate is required.");
        }

        if (!(value is DateTime startDate) || !(comparisonValue is DateTime birthDate))
        {
            return new ValidationResult("Invalid date format.");
        }

        
        if (startDate <= birthDate)
        {
            return new ValidationResult("Start date must be greater than birthdate.");
        }

        return ValidationResult.Success;
    }
}