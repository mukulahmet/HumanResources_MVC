using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class EmailValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required.");
        }

        string email = value.ToString();

        // Büyük harf olup olmadığını kontrol etme
        if (Regex.IsMatch(email, "[A-Z]"))
        {
            return new ValidationResult("Email cannot contain uppercase letters.");
        }

        // E-posta adresinin boşluk içerip içermediğini kontrol etme
        if (email.Contains(" "))
        {
            return new ValidationResult("Email cannot contain whitespace.");
        }

        // '@' ve '.' harici özel karakter olup olmadığını kontrol etme
        if (Regex.IsMatch(email, @"[^a-z0-9@.]"))
        {
            return new ValidationResult("Email can only contain lowercase letters, numbers, '@', and '.' characters.");
        }

        // '@' işaretinin bulunup bulunmadığını ve yalnızca bir tane olup olmadığını kontrol etme
        int atIndex = email.IndexOf('@');
        if (atIndex == -1)
        {
            return new ValidationResult("Email must contain the '@' symbol.");
        }

        // '@' işaretinden önce ve sonra en az iki karakter olmalı
        if (atIndex < 2 || atIndex >= email.Length - 2)
        {
            return new ValidationResult("Email must have at least two characters before and after the '@' symbol.");
        }

        // E-posta adresinin bir tane nokta içerip içermediğini kontrol etme
        if (email.Count(c => c == '.') != 1)
        {
            return new ValidationResult("Email must contain exactly one '.' symbol.");
        }

        if (!email.EndsWith(".com"))
        {
            return new ValidationResult("Email must end with '.com'.");
        }
   
        int domainPartLength = email.Length - atIndex - 5;
        if (domainPartLength < 2)
        {
            return new ValidationResult("Email must contain at least 2 characters between '@' and '.com'.");
        }

        return ValidationResult.Success;
    }
}