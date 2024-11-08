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

    public bool Any(Func<ConsoleCollectionItem, bool> predicate)
    {
        return _context
            .ConsoleCollectionItems
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Func<ConsoleCollectionItem, T> predicate, Guid id) where T : class
    {
        return _context.ConsoleCollectionItems
            .Where(x => x.Id == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public ConsoleCollectionItem SingleOrDefault(Func<ConsoleCollectionItem, bool> predicate)
    {
        return _context
            .ConsoleCollectionItems
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
