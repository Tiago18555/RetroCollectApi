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

    public bool Any(Func<UserCollection, bool> predicate)
    {
        return _context
            .UserCollections
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Guid id, Func<UserCollection, T> predicate) where T : class
    {
        return _context.UserCollections
            .Where(x => x.UserCollectionId == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public UserCollection SingleOrDefault(Func<UserCollection, bool> predicate)
    {
        return _context
            .UserCollections
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
