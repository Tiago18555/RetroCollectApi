using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.Shared
{
    public struct InternalConsole
    {
        [JsonPropertyName("id")]
        public int ConsoleId { get; set; }
        [JsonPropertyName("consoleName")]
        public string Name { get; set; }
    }
}
