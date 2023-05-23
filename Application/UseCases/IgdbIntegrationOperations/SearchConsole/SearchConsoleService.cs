using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole
{
    public class SearchConsoleService : ISearchConsoleService
    {
        private readonly HttpClient httpClient;
        public SearchConsoleService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            string query = "fields *, platform_logo.image_id, versions.name, versions.platform_logo.image_id, summary, url, websites.url, websites.category; where category = (1, 5); where id = " + id.ToString() + "; ";

            var res = await httpClient.IgdbPostAsync<List<PlatformResponseModel>>(query, "platforms");

            return res.Ok();
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> SearchBy(string name)
        {
            if (string.IsNullOrEmpty(name)) { return GenericResponses.NotFound("Field \"search cannot be empty\""); }

            string query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y; limit 50; where category = (1, 5); search \"{name.CleanKeyword()}\"; ";

            Console.WriteLine(query);

            var res = await httpClient.IgdbPostAsync<List<SearchConsoleResponseModel>>(query, "platforms");

            return res.Ok();
        }
    }
}
