using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole
{
    public interface ISearchConsoleService
    {
        Task<ResponseModel> SearchBy(string name);
        Task<ResponseModel> GetById(int id);
        Task<List<ConsoleInfo>> RetrieveConsoleInfoAsync(int game_id);
    }
}
