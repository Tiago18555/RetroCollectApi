using Domain.Broker;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Processor;

public class RequestProcessorFactory : IRequestProcessorFactory
{
    private readonly IServiceProvider _sp;

    public RequestProcessorFactory(IServiceProvider serviceProvider)
    {
        _sp = serviceProvider;
    }

    public IRequestProcessor Create(string s)
    {
        return s switch
        {
            "create-user" => _sp.GetService<CreateUserProcessor>(),
            _ => throw new ArgumentException($"Unknown message type: {s}")
        };
    }
}

