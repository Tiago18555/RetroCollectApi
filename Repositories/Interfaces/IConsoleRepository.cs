using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IConsoleRepository
    {
        Console Add(Console game);
        bool Any(Func<Console, bool> predicate);
    }
}
