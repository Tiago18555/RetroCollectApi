namespace Domain.Exceptions;
public class NullClaimException : Exception
{
    public NullClaimException(string message) : base(message)
    {

    }
}
