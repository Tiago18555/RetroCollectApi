using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
{
    public class UserComputerRepository : IUserComputerRepository
    {
        private readonly DataContext _context;

        public UserComputerRepository(DataContext context)
        {
            _context = context;
        }

        public UserComputer Add(UserComputer user)
        {
            _context.UserComputers.Add(user);
            _context.SaveChanges();

            return _context.UserComputers
                .Where(x => x.UserComputerId == user.UserComputerId)
                .FirstOrDefault();
        }

        public bool Any(Func<UserComputer, bool> predicate)
        {
            return _context.UserComputers.Any(predicate);
        }

        public bool Delete(UserComputer user)
        {
            _context.UserComputers.Remove(user);

            return !_context.UserComputers.Any(x => x.UserComputerId == user.UserComputerId); //NONE MATCH
        }

        public UserComputer GetById(Guid id)
        {
            return _context.UserComputers.Where(x => x.UserComputerId == id).FirstOrDefault();
        }

        public UserComputer SingleOrDefault(Func<UserComputer, bool> predicate)
        {
            return _context.UserComputers.Where(predicate).SingleOrDefault();
        }

        public UserComputer Update(UserComputer user)
        {
            _context.UserComputers.Update(user);
            _context.SaveChanges();

            return _context.UserComputers
                .Where(x => x.UserId == user.UserId)
                .FirstOrDefault();
        }
    }
}
