using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CrossCutting.Validations;

public class IsValidUri : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (!Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9_]+$"))
        {
            return new ValidationResult("Username can only contain letters, numbers, and underscores.");
        }
        return ValidationResult.Success;
    }
}