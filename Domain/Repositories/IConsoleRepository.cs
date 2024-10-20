using Console = Domain.Entities.Console;

namespace Domain.Repositories;

public interface IConsoleRepository
{
    /// <exception cref="ArgumentNullException"></exception>
    bool Any(Func<Console, bool> predicate);
}
