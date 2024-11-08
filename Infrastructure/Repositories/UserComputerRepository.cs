using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserComputerRepository : IUserComputerRepository
{
    private readonly DataContext _context;

    public UserComputerRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<ComputerCollectionItem, bool> predicate)
    {
        return _context
            .ComputerCollectionItems
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Guid id, Func<ComputerCollectionItem, T> predicate) where T : class
    {
        return _context.ComputerCollectionItems
            .Where(x => x.Id == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public ComputerCollectionItem SingleOrDefault(Func<ComputerCollectionItem, bool> predicate)
    {
        return _context
            .ComputerCollectionItems
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
