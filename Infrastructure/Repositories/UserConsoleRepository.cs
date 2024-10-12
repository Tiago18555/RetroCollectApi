using Domain.Entities;
using Application.Data;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
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
            _context.Entry(user).Reference(x => x.User).Load();
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

        public T GetById<T>(Func<UserConsole, T> predicate, Guid id) where T : class
        {
            return _context.UserConsoles
                .Where(x => x.UserConsoleId == id)
                .AsNoTracking()
                .Select(predicate)
                .FirstOrDefault();
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
