using System;
using System.Text.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Confluent.Kafka;
using Domain.Broker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Kafka;
public class KafkaConsumerService: IConsumerService
{
    private readonly IRequestProcessorFactory _processorFactory;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IConfiguration _configuration;
    private readonly Parameters _parameters;

    public KafkaConsumerService(
        ILogger<KafkaConsumerService> logger, 
        IRequestProcessorFactory processorFactory,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _processorFactory = processorFactory;
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

    private string GetMessageType(string message)
    {  
        System.Console.WriteLine("GET MESSAGE TYPE... ... ...");
        System.Console.WriteLine(message); 
        var _object = JsonSerializer.Deserialize(message, typeof(MessageModel)) as MessageModel;
        return _object.SourceType;
    }

    public async Task ConsumeAsync(CancellationToken cts)
    {
        _logger.LogInformation("Waiting messages");
        _consumer.Subscribe(_parameters.TopicName);

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(cts);
                var messageType = GetMessageType(result.Message.Value);
                var processor = _processorFactory.Create(messageType); //GET PROCESSOR TYPE

                var processResult = await processor.ProcessAsync(result.Message.Value); //CALL

                var data = JsonSerializer.Serialize(result.Message.Value);
                _logger.LogInformation($"GroupId: {_parameters.GroupId} Mensagem: {processResult}");
                _logger.LogInformation(data.ToString());                

                System.Console.WriteLine(data.ToString());
            }
            catch(ConsumeException)
            {
                _logger.LogInformation("There's no topic of this message.");
                System.Console.WriteLine("There's no topic of this message.");
            }
            catch(DbUpdateException e)
            {
                _logger.LogInformation($"An error occurs on the operation: {e.Message}");
                System.Console.WriteLine($"An error occurs on the operation: {e.Message}");
            }
            finally
            {
                
            }
        }
    }

    public Task StopAsync(CancellationToken cts)
    {
        _consumer.Close();
        _logger.LogInformation("Application stopped. Connection closed");
        return Task.CompletedTask;
    }

    public Task RetryAsync()
    {
        throw new NotImplementedException();
    }
}

