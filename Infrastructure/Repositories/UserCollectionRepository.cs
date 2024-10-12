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

    public UserCollection Add(UserCollection user)
    {
        _context.UserCollections.Add(user);
        _context.SaveChanges();
        _context.Entry(user).Reference(x => x.Game).Load();
        _context.Entry(user).Reference(x => x.User).Load();
        _context.Entry(user).State = EntityState.Detached; 

        return user;
    }

    public bool Any(Func<UserCollection, bool> predicate)
    {
        return _context
            .UserCollections
            .AsNoTracking()
            .Any(predicate);
    }

    public bool Delete(UserCollection user)
    {
        _context.UserCollections.Remove(user);
        _context.SaveChanges();

        return !_context.UserCollections.Any(x => x.UserCollectionId == user.UserCollectionId); //NONE MATCH
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

    public UserCollection Update(UserCollection user)
    {
        _context.UserCollections.Update(user);
        _context.Entry(user).Reference(x => x.Game).Load();
        _context.Entry(user).Reference(x => x.User).Load();
        _context.SaveChanges();

        return user;
    }
}
