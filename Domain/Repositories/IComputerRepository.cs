using Domain.Entities;

namespace Domain.Repositories;

public interface IComputerRepository
{
    /// <exception cref="DbUpdateConcurrencyException"></exception>
    /// <exception cref="DbUpdateException"></exception>
    /// <returns>The entity found, or <see langword="null" />.</returns>
    Computer Add(Computer game);

    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Computer, bool> predicate);
}
