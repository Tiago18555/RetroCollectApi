using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.Shared
{
    public struct InternalComputer
    {
        [JsonPropertyName("id")]
        public int ComputerId { get; set; }
        [JsonPropertyName("computerName")]
        public string Name { get; set; }
    }
}
