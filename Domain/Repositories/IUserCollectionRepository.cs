using Domain.Entities;

namespace Domain.Repositories;

public interface IUserCollectionRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Guid id, Func<UserCollection, T> predicate) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<UserCollection, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    UserCollection SingleOrDefault(Func<UserCollection, bool> predicate);
}
