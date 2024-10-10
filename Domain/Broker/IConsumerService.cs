public interface IConsumerService
{
    /// <exception cref="ConsumeException"></exception>
    /// <exception cref="KafkaException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    void Consume<T>(CancellationToken cts);

    Task StopAsync(CancellationToken cts);
}
