using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations
{
    public interface ISearchGameService
    {
        Task<ResponseModel> SearchBy(string name, string genre, string keyword, string companie, string language, string theme, string releaseyear);
    }
}
