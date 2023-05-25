using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IConsoleRepository
    {
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        /// <returns>The entity found, or <see langword="null" />.</returns>
        Console Add(Console game);

        /// <exception cref="ArgumentNullException"></exception>
        bool Any(Func<Console, bool> predicate);
    }
}
