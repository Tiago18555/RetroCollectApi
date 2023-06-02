using Microsoft.EntityFrameworkCore;
using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
{
    public class UserConsoleRepository : IUserConsoleRepository
    {
        private readonly DataContext _context;

        public UserConsoleRepository(DataContext context)
        {
            _context = context;
        }

        public UserConsole Add(UserConsole user)
        {
            _context.UserConsoles.Add(user);
            _context.SaveChanges();
            _context.Entry(user).Reference(x => x.Console).Load();
            _context.Entry(user).State = EntityState.Detached;

            return user;
        }

        public bool Any(Func<UserConsole, bool> predicate)
        {
            return _context
                .UserConsoles
                .AsNoTracking()
                .Any(predicate);
        }

        public bool Delete(UserConsole user)
        {
            _context.UserConsoles.Remove(user);
            _context.SaveChanges();

            return !_context.UserConsoles.Any(x => x.UserConsoleId == user.UserConsoleId); //NONE MATCH
        }

        public UserConsole GetById(Guid id)
        {
            return _context.UserConsoles.Where(x => x.UserConsoleId == id).AsNoTracking().FirstOrDefault();
        }

        public UserConsole SingleOrDefault(Func<UserConsole, bool> predicate)
        {
            return _context
                .UserConsoles
                .Where(predicate)
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefault();
        }

        public UserConsole Update(UserConsole user)
        {
            _context.UserConsoles.Update(user);
            _context.Entry(user).Reference(x => x.User).Load();
            _context.SaveChanges();

            return user;
        }
    }
}
