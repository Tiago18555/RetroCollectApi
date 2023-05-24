using RetroCollect.Models;

namespace RetroCollectApi.Repositories.Interfaces
{
    public interface IComputerRepository
    {
        Computer Add(Computer game);
        bool Any(Func<Computer, bool> predicate);
    }
}
