using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using System.Linq;

namespace RetroCollectApi.Repositories.Interfaces
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

        /// <exception cref="ArgumentNullException"></exception>
        List<T> GetAllComputersByUser<T>(Guid userId, Func<UserComputer, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        List<T> GetAllConsolesByUser<T>(Guid userId, Func<UserConsole, T> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        List<T> GetAllCollectionsByUser<T>(Guid userId, Func<UserCollection, T> predicate);

        #endregion

    }
}
