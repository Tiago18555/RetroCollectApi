using Domain.Entities;

namespace Domain.Repositories;

public interface IGameRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Game, bool> predicate);
}
