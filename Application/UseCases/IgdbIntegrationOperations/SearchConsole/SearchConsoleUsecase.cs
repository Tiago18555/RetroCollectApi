using CrossCutting;
using Infrastructure;
using Application.UseCases.IgdbIntegrationOperations.Shared;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchConsole
{
    public class SearchConsoleUsecase : ISearchConsoleUsecase
    {
        private readonly HttpClient _httpClient;
        public SearchConsoleUsecase()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            string query = "fields *, platform_logo.image_id, versions.name, versions.platform_logo.image_id, summary, url, websites.url, websites.category; where category = (1, 5); where id = " + id.ToString() + "; ";

            var res = await _httpClient.IgdbPostAsync<List<PlatformResponseModel>>(query, "platforms");

            return res.Ok();
        }

        public async Task<List<ConsoleInfo>> RetrieveConsoleInfoAsync(int game_id)
        {
            String query = @"
                fields                         
                name, 
                summary,
                platform_logo.image_id,
                category;
                where id = " + game_id.ToString() + ";";

            var res = await _httpClient.IgdbPostAsync<List<ConsoleInfo>>(query, "platforms");

            return res;
        }

        public async Task<ResponseModel> SearchBy(string name, int limit = 0)
        {
            if (string.IsNullOrEmpty(name)) { return ResponseFactory.NotFound("Field \"search cannot be empty\""); }

            string query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y; limit 50; where category = (1, 5); search \"{name.CleanKeyword()}\"; limit {limit};";

            if(limit == 0)
                query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y; limit 50; where category = (1, 5); search \"{name.CleanKeyword()}\"; ";

            Console.WriteLine(query);

            var res = await _httpClient.IgdbPostAsync<List<SearchConsoleResponseModel>>(query, "platforms");

            return res.Ok();
        }
    }

    public class ConsoleInfo
    {
        [JsonProperty("id")]
        public int ConsoleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Description { get; set; }

        [JsonProperty("platform_logo")]
        private Platform_Logo Platform_Logo { get; set; }
        public string ImageUrl => Platform_Logo.Image_Id;

        [JsonProperty("category")]
        private int Category { get; set; }
        public bool IsPortable => Category == 5;
    }
}
