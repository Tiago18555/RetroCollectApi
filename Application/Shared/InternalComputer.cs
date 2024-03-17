using System.Text.Json.Serialization;

namespace Application.Shared
{
    public struct InternalComputer
    {
        [JsonPropertyName("id")]
        public int ComputerId { get; set; }
        [JsonPropertyName("computerName")]
        public string Name { get; set; }
    }
}
