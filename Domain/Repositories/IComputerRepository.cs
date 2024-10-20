using Domain.Entities;

namespace Domain.Repositories;

public interface IComputerRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Computer, bool> predicate);
}
