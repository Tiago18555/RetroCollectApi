using RetroCollect.Models;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.GameOperations.Shared
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

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
