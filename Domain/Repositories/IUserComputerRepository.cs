using Domain.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IUserComputerRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserComputer Add(UserComputer user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><see langword="true" /> if the entity has deleted successfully</returns>
        bool Delete(UserComputer user);

        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>returns the Entity found or the default value for entity</returns>
        T GetById<T>(Guid id, Func<UserComputer, T> predicate) where T : class;

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<UserComputer, bool> predicate);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        UserComputer SingleOrDefault(Func<UserComputer, bool> predicate);

        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        UserComputer Update(UserComputer user);
    }
}
