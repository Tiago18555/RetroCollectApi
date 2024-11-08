using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCollectionRepository : IUserCollectionRepository
{
    private readonly DataContext _context;

    public UserCollectionRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<GameCollectionItem, bool> predicate)
    {
        return _context
            .GameCollectionItems
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Guid id, Func<GameCollectionItem, T> predicate) where T : class
    {
        return _context.GameCollectionItems
            .Where(x => x.Id == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public GameCollectionItem SingleOrDefault(Func<GameCollectionItem, bool> predicate)
    {
        return _context
            .GameCollectionItems
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
