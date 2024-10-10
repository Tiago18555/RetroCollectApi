public interface IRequestProcessor
{
    Task ProcessAsync(string message);
}
