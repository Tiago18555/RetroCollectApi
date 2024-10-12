namespace Domain.Exceptions;
public class InvalidEnumValueException : Exception
{
    public InvalidEnumValueException(string message) : base(message)
    {

    }
}
