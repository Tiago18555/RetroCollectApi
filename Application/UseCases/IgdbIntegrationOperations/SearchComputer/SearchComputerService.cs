using CrossCutting;
using Application.UseCases.IgdbIntegrationOperations.Shared;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public class SearchComputerService : ISearchComputerService
    {
        private readonly HttpClient httpClient;
        public SearchComputerService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            string query = $"fields *, platform_logo.image_id, versions.name, versions.platform_logo.image_id, summary, url, websites.url, websites.category; where category = (2, 6); where id = {id.ToString()};";

            var res = await httpClient.IgdbPostAsync<List<PlatformResponseModel>>(query, "platforms");

            return res.Ok();
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> SearchBy(string name, int limit)
        {
            if (string.IsNullOrEmpty(name)) { return GenericResponses.NotFound("Field \"search cannot be empty\""); }

            string query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y;\r\nlimit 50;\r\nwhere category = (2, 6);\r\nsearch \"{name.CleanKeyword()}\"; limit {limit};";

            if(limit == 0)
                query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y;\r\nlimit 50;\r\nwhere category = (2, 6);\r\nsearch \"{name.CleanKeyword()}\";";

            Console.WriteLine(query);

            var res = await httpClient.IgdbPostAsync<List<SearchComputerResponseModel>>(query, "platforms");

            return res.Ok();
        }

        public async Task<List<ComputerInfo>> RetrieveComputerInfoAsync(int game_id)
        {
            String query = @"
                fields                         
                name, 
                summary,
                platform_logo.image_id,
                category;
                where id = " + game_id.ToString() + ";";

            var res = await httpClient.IgdbPostAsync<List<ComputerInfo>>(query, "platforms");

            return res;
        }
    }
    public class ComputerInfo
    {
        [JsonProperty("id")]
        public int ComputerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Description { get; set; }

        [JsonProperty("platform_logo")]
        private Platform_Logo Platform_Logo { get; set; }
        public string ImageUrl => Platform_Logo.Image_Id;

        [JsonProperty("category")]
        private int Category { get; set; }
        public bool IsArcade => Category == 2;
    }
}
