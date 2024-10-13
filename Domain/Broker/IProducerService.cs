namespace Domain.Broker;

public interface IProducerService
{
    /// <exception cref="ProduceException"></exception>
    /// <exception cref="ArgumentException"></exception>
    Task< (string Status, string Message) > SendMessage(string message);
}
