namespace Domain.Broker;
public interface IRequestProcessor
{
    Task<MessageModel> ProcessAsync(string message);
}
