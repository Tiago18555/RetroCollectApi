using System.Runtime.CompilerServices;

namespace Domain.Exceptions
{
    public class InvalidClassTypeException : Exception
    {
        public InvalidClassTypeException(
            string message, 
            [CallerLineNumber] int lineNumber = 0, 
            [CallerMemberName] string caller = null,
            [CallerFilePath] string filePath = null
        ) : base(message = $"Error: Invalid Class Type on {filePath}, line: {lineNumber}, on caller: {caller}")
        {

        }
    }

}
