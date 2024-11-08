using Domain.Entities;

namespace Domain.Repositories;

public interface IUserConsoleRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Func<ConsoleCollectionItem, T> predicate, Guid id) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<ConsoleCollectionItem, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    ConsoleCollectionItem SingleOrDefault(Func<ConsoleCollectionItem, bool> predicate);
}
