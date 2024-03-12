using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Providers;
using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.CrossCutting.Validations
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        //IDateTimeProvider _dateTimeProvider; 

        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                if ((DateTime)value == DateTime.MinValue) return true;
                return date <= DateTime.UtcNow;
            }

            return false;
        }
    }
}

