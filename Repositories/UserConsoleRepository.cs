using RetroCollect.Data;
using RetroCollect.Models;

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

            return _context.UserConsoles
                .Where(x => x.UserConsoleId == user.UserConsoleId)
                .FirstOrDefault();
        }

        public bool Any(Func<UserConsole, bool> predicate)
        {
            return _context.UserConsoles.Any(predicate);
        }

        public bool Delete(UserConsole user)
        {
            _context.UserConsoles.Remove(user);

            return !_context.UserConsoles.Any(x => x.UserConsoleId == user.UserConsoleId); //NONE MATCH
        }

        public UserConsole GetById(Guid id)
        {
            return _context.UserConsoles.Where(x => x.UserConsoleId == id).FirstOrDefault();
        }

        public UserConsole SingleOrDefault(Func<UserConsole, bool> predicate)
        {
            return _context.UserConsoles.Where(predicate).SingleOrDefault();
        }
    }
}
