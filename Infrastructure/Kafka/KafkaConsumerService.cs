using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka;
public class KafkaConsumerService: IConsumerService
{
    private readonly IConfiguration _configuration;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ConsumerConfig _consumerConfig;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly Parameters _parameters;
    
    //private readonly Dictionary<string, IRequestProcessor> _processors;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _parameters = new Parameters();
        _configuration = configuration;

        _parameters.BootstrapServer = _configuration
            .GetSection("KafkaConfig")
            .GetSection("BootstrapServer").Value;

        _consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = _parameters.BootstrapServer,
            GroupId = _parameters.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

    }

    public void Consume<T>(CancellationToken cts)
    {
        _logger.LogInformation("Waiting messages");
        _consumer.Subscribe(_parameters.TopicName);

        while (!cts.IsCancellationRequested)
        {
            var result = _consumer.Consume(cts);
            var data = JsonSerializer.Deserialize<T> (result.Message.Value);
            _logger.LogInformation($"GroupId: {_parameters.GroupId} Mensagem: {result.Message.Value}");
            _logger.LogInformation(data.ToString());

            System.Console.WriteLine(data.ToString());
        }
    }

    public Task StopAsync(CancellationToken cts)
    {
        _consumer.Close();
        _logger.LogInformation("Application stopped. Connection closed");
        return Task.CompletedTask;
    }
}
