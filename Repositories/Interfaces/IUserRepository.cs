using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RetroCollect.Data;
using RetroCollect.Models;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
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

    }
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User Add(User user)
        {
            this._context.Users.Add(user);
            _context.SaveChanges();

            return _context.Users.Find(user.UserId);
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
            return _context.Users.SingleOrDefault(predicate);
        }
    }
}
