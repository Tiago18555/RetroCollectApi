using System.Text.Json.Serialization;

namespace Application.Shared;

public struct InternalUser
{
    [JsonPropertyName("id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("userName")]
    public string Username { get; set; }
}
