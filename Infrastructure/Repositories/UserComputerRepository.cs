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

    public bool Any(Func<UserComputer, bool> predicate)
    {
        return _context
            .UserComputers
            .AsNoTracking()
            .Any(predicate);
    }

    public T GetById<T>(Guid id, Func<UserComputer, T> predicate) where T : class
    {
        return _context.UserComputers
            .Where(x => x.UserComputerId == id)
            .AsNoTracking()
            .Select(predicate)
            .FirstOrDefault();
    }

    public UserComputer SingleOrDefault(Func<UserComputer, bool> predicate)
    {
        return _context
            .UserComputers
            .Where(predicate)
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefault();
    }
}
