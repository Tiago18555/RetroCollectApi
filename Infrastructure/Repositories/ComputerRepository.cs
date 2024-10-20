using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ComputerRepository : IComputerRepository
{
    private readonly DataContext _context;

    public ComputerRepository(DataContext context)
    {
        _context = context;
    }

    public bool Any(Func<Computer, bool> predicate)
    {
        return _context.Computers.AsNoTracking().Any(predicate);
    }
}
