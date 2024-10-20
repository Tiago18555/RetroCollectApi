using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly DataContext _context;

    public GameRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<Game, bool> predicate)
    {
        return _context
            .Games
            .AsNoTracking()
            .Any(predicate);
    }
}
