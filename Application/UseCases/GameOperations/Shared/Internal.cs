using RetroCollect.Models;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.GameOperations.Shared
{
    public struct InternalGame
    {
        public int GameId { get; set; }
        public string Title { get; set; }
    }

    public struct InternalUser
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
