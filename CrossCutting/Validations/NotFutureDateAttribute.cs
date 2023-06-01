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
                if ((DateTime)value == DateTime.MinValue) return true;
                return date <= DateTime.Now;
            }

            return false;
        }
    }
}

