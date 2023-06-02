using Microsoft.EntityFrameworkCore;
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
            _context.Entry(user).Reference(x => x.Computer).Load();
            _context.Entry(user).State = EntityState.Detached;

            return user;
        }

        public bool Any(Func<UserComputer, bool> predicate)
        {
            return _context
                .UserComputers
                .AsNoTracking()
                .Any(predicate);
        }

        public bool Delete(UserComputer user)
        {
            _context.UserComputers.Remove(user);
            _context.SaveChanges();

            return !_context.UserComputers.Any(x => x.UserComputerId == user.UserComputerId); //NONE MATCH
        }

        public UserComputer GetById(Guid id)
        {
            return _context.UserComputers.Where(x => x.UserComputerId == id).AsNoTracking().FirstOrDefault();
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

        public UserComputer Update(UserComputer user)
        {
            _context.UserComputers.Update(user);;
            _context.Entry(user).Reference(x => x.User).Load();
            _context.SaveChanges();

            return user;
        }
    }
}
