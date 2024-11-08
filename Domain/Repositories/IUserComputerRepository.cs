using Domain.Entities;

namespace Domain.Repositories;

public interface IUserComputerRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Guid id, Func<ComputerCollectionItem, T> predicate) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<ComputerCollectionItem, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    ComputerCollectionItem SingleOrDefault(Func<ComputerCollectionItem, bool> predicate);
}
