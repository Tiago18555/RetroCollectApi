using Application.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Console = Domain.Entities.Console;

namespace Application.Repositories
{
    public class ConsoleRepository : IConsoleRepository
    {
        private readonly DataContext _context;

        public ConsoleRepository(DataContext context)
        {
            _context = context;
        }

        public Console Add(Console console)
        {
            _context.Consoles.Add(console);
            _context.SaveChanges();
            _context.Entry(console).State = EntityState.Detached;

            return console;
        }

        public bool Any(Func<Console, bool> predicate)
        {
            return _context.Consoles.AsNoTracking().Any(predicate);
        }
    }
}
