using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public interface ISearchComputerService
    {
        Task<ResponseModel> SearchBy(string name);
        Task<ResponseModel> GetById(int id);
    }
}
