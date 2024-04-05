using CrossCutting;

namespace Application.UseCases.IgdbIntegrationOperations.SearchConsole
{
    public interface ISearchConsoleService
    {
        Task<ResponseModel> SearchBy(string name, int limit);
        Task<ResponseModel> GetById(int id);
        Task<List<ConsoleInfo>> RetrieveConsoleInfoAsync(int game_id);
    }
}
