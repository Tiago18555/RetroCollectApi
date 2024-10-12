namespace Domain.Broker;

public interface IConsumerService
{
    /// <exception cref="ConsumeException"></exception>
    /// <exception cref="KafkaException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    Task ConsumeAsync(CancellationToken cts);

    Task StopAsync(CancellationToken cts);

    Task RetryAsync();
}
