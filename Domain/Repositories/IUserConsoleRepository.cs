using Domain.Entities;

namespace Domain.Repositories.Interfaces
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
        T GetById<T>(Func<UserConsole, T> predicate, Guid id) where T : class;

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<UserConsole, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        UserConsole SingleOrDefault(Func<UserConsole, bool> predicate);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserConsole Update(UserConsole user);
    }
}
