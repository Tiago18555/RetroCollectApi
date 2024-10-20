using Domain.Entities;

namespace Domain.Repositories;

public interface IUserComputerRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Guid id, Func<UserComputer, T> predicate) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<UserComputer, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    UserComputer SingleOrDefault(Func<UserComputer, bool> predicate);
}
