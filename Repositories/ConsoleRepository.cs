using Microsoft.EntityFrameworkCore;
using RetroCollect.Data;
using RetroCollectApi.Repositories.Interfaces;
using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Repositories
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
