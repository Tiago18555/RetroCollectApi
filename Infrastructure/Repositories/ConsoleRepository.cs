using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Console = Domain.Entities.Console;

namespace Infrastructure.Repositories;

public class ConsoleRepository : IConsoleRepository
{
    private readonly DataContext _context;

    public ConsoleRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<Console, bool> predicate)
    {
        return _context.Consoles.AsNoTracking().Any(predicate);
    }
}
