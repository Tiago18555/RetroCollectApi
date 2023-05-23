using RetroCollectApi.CrossCutting;
using System.Net.Http;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame
{
    public interface ISearchGameService
    {
        Task<ResponseModel> SearchBy(string name, string genre, string keyword, string companie, string language, string theme, string releaseyear);

        Task<ResponseModel> GetById(int id);
    }
}
