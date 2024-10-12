namespace Domain.Broker;

public interface IRequestProcessorFactory
{
    IRequestProcessor Create(string messageType);
}

