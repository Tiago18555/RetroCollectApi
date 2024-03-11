namespace RetroCollectApi.CrossCutting.Providers
{
    public class DateTimeProvider: IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public long GetCurrentTimestampSeconds()
        {
            return (long)(UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
