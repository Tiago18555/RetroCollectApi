using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers;
using RetroCollect.Data;
using RetroCollect.Models;
using System.Linq;

namespace RetroCollectApi.Repositories
{
    public interface IUserRepository
    {
        #region Personal info
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        User Add(User user);

        /// <exception cref="ArgumentNullException"></exception>
        List<T> GetAll<T>(Func<User, T> predicate);


        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<User, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        User SingleOrDefault(Func<User, bool> predicate);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        User Update(User user);

        #endregion

        #region User Collections

        List<T> GetAllComputersByUser<T>(Guid userId, Func<UserComputer, T> predicate);
        List<T> GetAllConsolesByUser<T>(Guid userId, Func<UserConsole, T> predicate);
        List<T> GetAllCollectionsByUser<T>(Guid userId, Func<UserCollection, T> predicate);

        #endregion

    }
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
                .FirstOrDefault();
        }

        public List<T> GetAll<T>(Func<User, T> predicate)
        {
            return _context.Users
                .Select(predicate)
                .ToList();
        }

        public bool Any(Func<User, bool> predicate)
        {
            return _context.Users.Any(predicate);
        }

        public User SingleOrDefault(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate).SingleOrDefault();
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return _context.Users
                .Where(x => x.UserId == user.UserId)
                .FirstOrDefault();
        }

        public List<T> GetAllComputersByUser<T>(Guid userId, Func<UserComputer, T> predicate)
        {
            return _context.UserComputers
                .Include(x => x.Computer)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList();
        }

        public List<T> GetAllConsolesByUser<T>(Guid userId, Func<UserConsole, T> predicate)
        {
            return _context.UserConsoles
                .Include(x => x.Console)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(predicate)
                .ToList();
        }

        public List<T> GetAllCollectionsByUser<T>(Guid userId, Func<UserCollection, T> predicate)
        {
            return _context.UserCollections
                 .Include(x => x.Game)
                 .AsNoTracking()
                 .Where(x => x.UserId == userId)
                 .Select(predicate)
                 .ToList();
        }
    }
}
