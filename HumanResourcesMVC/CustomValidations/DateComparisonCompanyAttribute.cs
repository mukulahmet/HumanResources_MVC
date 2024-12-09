using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HumanResourcesMVC.CustomValidations
{
    public class DateComparisonCompanyAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateComparisonCompanyAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyName = validationContext.MemberName;
            var formattedPropertyName = Regex.Replace(propertyName, "(\\B[A-Z])", " $1").Trim();

            if (value == null)
            {
                return new ValidationResult($"{formattedPropertyName} is required.");
            }

            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonProperty == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);
            var formattedComparisonPropertyName = Regex.Replace(_comparisonProperty, "(\\B[A-Z])", " $1").Trim();

            if (comparisonValue == null)
            {
                return new ValidationResult($"{formattedComparisonPropertyName} is required.");
            }

            if (!(value is DateTime startDate) || !(comparisonValue is DateTime endDate))
            {
                return new ValidationResult("Invalid date format.");
            }


            if (startDate >= endDate)
            {
                return new ValidationResult($"{formattedPropertyName} must be smaller than {formattedComparisonPropertyName}.");
            }

            return ValidationResult.Success;
        }
    }
}
