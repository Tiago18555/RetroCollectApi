namespace Application.CrossCutting.Providers
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        long GetCurrentTimestampSeconds();
    }
}
