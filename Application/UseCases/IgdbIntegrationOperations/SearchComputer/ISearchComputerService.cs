using Application.CrossCutting;

namespace Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public interface ISearchComputerService
    {
        Task<ResponseModel> SearchBy(string name);
        Task<ResponseModel> GetById(int id);
        Task<List<ComputerInfo>> RetrieveComputerInfoAsync(int game_id);

    }
}
