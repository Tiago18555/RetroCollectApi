using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
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

            return _context.Computers
                .Where(x => x.ComputerId == computer.ComputerId)
                .FirstOrDefault();
        }

        public bool Any(Func<Computer, bool> predicate)
        {
            return _context.Computers.Any(predicate);
        }
    }
}
