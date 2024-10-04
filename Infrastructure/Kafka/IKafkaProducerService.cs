public interface IKafkaProducerService
{
    /// <exception cref="ProduceException"></exception>
    /// <exception cref="ArgumentException"></exception>
    Task ProduceAsync(string topic, string message);
}