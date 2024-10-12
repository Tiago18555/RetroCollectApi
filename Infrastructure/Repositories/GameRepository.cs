using Domain.Entities;
using Application.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
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
            _context.Entry(game).State = EntityState.Detached;

            return game;
        }

        public bool Any(Func<Game, bool> predicate)
        {
            return _context
                .Games
                .AsNoTracking()
                .Any(predicate);
        }
    }
}
