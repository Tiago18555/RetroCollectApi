using System.Text.Json.Serialization;

namespace Application.Shared
{
    public struct InternalGame
    {
        [JsonPropertyName("id")]
        public int GameId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
