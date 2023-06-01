using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.CrossCutting.Validations
{
    public class IsValidOwnershipStatus : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            return value.ToString().ToLower().Trim() switch
            {
                "owned" => ValidationResult.Success,
                "desired" => ValidationResult.Success,
                "traded" => ValidationResult.Success,
                "borrowed" => ValidationResult.Success,
                "sold" => ValidationResult.Success,
                _ => new ValidationResult("Please enter a correct value: \"owned\", \"desired\", \"traded\", \"borrowed\" or \"sold\"")
            };
        }
    }
}

