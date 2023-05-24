using RetroCollect.Models;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Game Add(Game game);
        bool Any(Func<Game, bool> predicate);
    }
}
