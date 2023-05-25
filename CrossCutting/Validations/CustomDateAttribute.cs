using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.CrossCutting.Validations
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date != DateTime.MinValue;
            }

            return false;
        }
    }

}
