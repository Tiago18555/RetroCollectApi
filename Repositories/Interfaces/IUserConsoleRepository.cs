using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IUserConsoleRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserConsole Add(UserConsole user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see langword="true" /> if the entity has deleted successfully</returns>
        bool Delete(UserConsole user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>returns the Entity found or the default value for entity</returns>
        UserConsole GetById(Guid id);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<UserConsole, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        UserConsole SingleOrDefault(Func<UserConsole, bool> predicate);
    }
}
