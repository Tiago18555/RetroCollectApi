using RetroCollect.Models;
using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.CrossCutting.Validations
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Now;
            }

            return false;
        }
    }
}

