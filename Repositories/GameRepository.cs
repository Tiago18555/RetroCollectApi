using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public Game Add(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();

            return _context.Games
                .Where(x => x.GameId == game.GameId)
                .FirstOrDefault();
        }

        public bool Any(Func<Game, bool> predicate)
        {
            return _context.Games.Any(predicate);
        }
    }
}
