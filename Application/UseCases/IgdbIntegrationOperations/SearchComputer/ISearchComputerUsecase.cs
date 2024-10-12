using Domain;

namespace Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public interface ISearchComputerUsecase
    {
        Task<ResponseModel> SearchBy(string name, int limit);
        Task<ResponseModel> GetById(int id);
        Task<List<ComputerInfo>> RetrieveComputerInfoAsync(int game_id);

    }
}
