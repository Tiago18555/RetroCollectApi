using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Validations;

public class IsValidCondition : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        return value.ToString().ToLower().Trim() switch
        {
            "new" => ValidationResult.Success,
            "likenew" => ValidationResult.Success,
            "used" => ValidationResult.Success,
            "fair" => ValidationResult.Success,
            "poor" => ValidationResult.Success,
            _ => new ValidationResult("Please enter a correct value: \n\"new\", \"likenew\", \"used\", \"fair\" or \"poor\"")
        };
    }
}

