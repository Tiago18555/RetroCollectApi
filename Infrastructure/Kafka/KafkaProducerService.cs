using Confluent.Kafka;

public class KafkaProducerService: IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var result = await _producer.ProduceAsync(topic, new Message<string, string> { Value = message });
        Console.WriteLine($"Mensagem entregue no tópico {result.Topic} com a chave {result.Key}");
    }
}
