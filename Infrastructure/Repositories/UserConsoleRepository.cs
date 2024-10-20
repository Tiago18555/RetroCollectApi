using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserConsoleRepository : IUserConsoleRepository
{
    private readonly DataContext _context;

    public UserConsoleRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<UserConsole, bool> predicate)
    {
        return _context
            .UserConsoles
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Func<UserConsole, T> predicate, Guid id) where T : class
    {
        return _context.UserConsoles
            .Where(x => x.UserConsoleId == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public UserConsole SingleOrDefault(Func<UserConsole, bool> predicate)
    {
        return _context
            .UserConsoles
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
