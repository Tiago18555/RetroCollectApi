using System.Text.Json;
using Confluent.Kafka;
using CrossCutting;
using Domain.Broker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka;

public class KafkaProducerService: IProducerService
{
    private readonly IConfiguration _configuration;
    private readonly ProducerConfig _producerConfig;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IConfiguration conf, ILogger<KafkaProducerService> logger)
    {
        _configuration = conf;
        _logger = logger;

        var bootstrap = _configuration.GetSection("KafkaConfig").GetSection("BootstrapServer").Value;

        _producerConfig = new ProducerConfig()
        {
            BootstrapServers = bootstrap
        };
    }

    public async Task<(string Status, string Message)> SendMessage(string message, string topic)
    {       

        try
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            var result = await producer.ProduceAsync(topic: topic, new() { Value = message });
            _logger.LogInformation(result.Status.ToString() + " - " + message);

            return (result.Status.ToString(), message);
        }
        catch (Exception e)
        {
            StdOut.Error($"Erro ao enviar mensagem: {e.Message}");
            return ("Error", "Erro ao enviar mensagem");
        }
    }
}
