using Confluent.Kafka;

public class KafkaConsumerService: IKafkaConsumerService
{
    private readonly IConsumer<string, string> _consumer;

    public KafkaConsumerService(string bootstrapServers, string groupId, string topic)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(topic);
    }

    public void Consume(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken).Message.Value;
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }
}
