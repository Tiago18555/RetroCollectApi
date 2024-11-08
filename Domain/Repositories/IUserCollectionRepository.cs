using Domain.Entities;

namespace Domain.Repositories;

public interface IUserCollectionRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>returns the Entity found or the default value for entity</returns>
    T GetById<T>(Guid id, Func<GameCollectionItem, T> predicate) where T : class;

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<GameCollectionItem, bool> predicate);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    GameCollectionItem SingleOrDefault(Func<GameCollectionItem, bool> predicate);
}
