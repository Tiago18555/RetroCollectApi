using Domain.Entities;

namespace Domain.Repositories;

public interface IUserConsoleRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Func<UserConsole, T> predicate, Guid id) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<UserConsole, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    UserConsole SingleOrDefault(Func<UserConsole, bool> predicate);
}
