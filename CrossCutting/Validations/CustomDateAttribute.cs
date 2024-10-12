using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                Console.WriteLine(value);
                Console.WriteLine(DateTime.MinValue);
                Console.WriteLine((DateTime)value == DateTime.MinValue);
                if ((DateTime)value == DateTime.MinValue) return true;
                return date != DateTime.MinValue;
            }

            return false;
        }
    }

}
