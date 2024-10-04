public interface IKafkaConsumerService
{
    /// <exception cref="ConsumeException"></exception>
    /// <exception cref="KafkaException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    void Consume(CancellationToken cancellationToken);
}
