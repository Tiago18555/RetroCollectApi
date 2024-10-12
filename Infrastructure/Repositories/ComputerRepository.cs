using Domain.Entities;
using Domain.Repositories.Interfaces;
using Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class ComputerRepository : IComputerRepository
    {
        private readonly DataContext _context;

        public ComputerRepository(DataContext context)
        {
            _context = context;
        }

        public Computer Add(Computer computer)
        {
            _context.Computers.Add(computer);
            _context.SaveChanges();
            _context.Entry(computer).State = EntityState.Detached;

            return computer;
        }

        public bool Any(Func<Computer, bool> predicate)
        {
            return _context.Computers.AsNoTracking().Any(predicate);
        }
    }
}
