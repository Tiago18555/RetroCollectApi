namespace Domain.Broker;

public class MessageModel
{
    public string SourceType { get; set; }
    public object Message { get; set; }
}

public class MessageModel<T>
{
    public string SourceType { get; set; }
    public T Message { get; set; }
}

