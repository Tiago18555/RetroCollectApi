using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.Shared
{
    public struct InternalGame
    {
        [JsonPropertyName("id")]
        public int GameId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public struct InternalUser
    {
        [JsonPropertyName("id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("userName")]
        public string Username { get; set; }
    }
    public struct InternalComputer
    {
        [JsonPropertyName("id")]
        public int ComputerId { get; set; }
        [JsonPropertyName("computerName")]
        public string Name { get; set; }
    }
    public struct InternalConsole
    {
        [JsonPropertyName("id")]
        public int ConsoleId { get; set; }
        [JsonPropertyName("consoleName")]
        public string Name { get; set; }
    }
}
