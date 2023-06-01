using Microsoft.EntityFrameworkCore;
using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.Repositories.Interfaces;

namespace RetroCollectApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return _context.Users
                .Where(x => x.UserId == user.UserId)
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefault();
        }

        public async Task<List<T>> GetAll<T>(Func<User, T> predicate)
        {
            return await Task.FromResult(_context.Users
                .Select(predicate)
                .ToList());
        }

        public bool Any(Func<User, bool> predicate)
        {
            return _context.Users.AsNoTracking().Any(predicate);
        }

        public User SingleOrDefault(Func<User, bool> predicate)
        {
            return _context
                .Users
                .Where(predicate)
                .AsQueryable()
                .AsNoTracking()
                .SingleOrDefault();
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return _context.Users
                .Where(x => x.UserId == user.UserId)
                .FirstOrDefault();
        }

        public async Task<List<T>> GetAllComputersByUser<T>(Guid userId, Func<UserComputer, T> predicate)
        {
            return await Task.FromResult(_context.UserComputers
                .Include(x => x.Computer)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList());
        }

        public async Task<List<T>> GetAllConsolesByUser<T>(Guid userId, Func<UserConsole, T> predicate)
        {
            return await Task.FromResult(_context.UserConsoles
                .Include(x => x.Console)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList());
        }

        public async Task<List<T>> GetAllCollectionsByUser<T>(Guid userId, Func<UserCollection, T> predicate)
        {
            return await Task.FromResult(_context.UserCollections
                 .Include(x => x.Game)
                 .AsNoTracking()
                 .Where(x => x.UserId == userId)
                 .Select(predicate)
                 .ToList());
        }
    }
}
