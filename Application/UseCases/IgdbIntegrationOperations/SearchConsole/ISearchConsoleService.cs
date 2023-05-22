using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole
{
    public interface ISearchConsoleService
    {
        Task<ResponseModel> SearchBy(string name);
    }
}
