using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.CrossCutting.Validations
{
    public class GreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int i;
            return value == null || int.TryParse(value.ToString(), out i) && i > 0;
        }
    }

}
