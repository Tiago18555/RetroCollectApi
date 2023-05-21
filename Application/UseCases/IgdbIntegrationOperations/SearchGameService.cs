using Newtonsoft.Json;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations
{
    public class SearchGameService : ISearchGameService
    {
        private readonly HttpClient httpClient;

        public SearchGameService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> SearchBy(string search, string genre, string keyword, string companie, string language, string theme, string releaseyear)
        {

            var queryParams = new Dictionary<string, string>();


            if (!string.IsNullOrEmpty(search))
                queryParams["search"] = search;

            if (!string.IsNullOrEmpty(genre))
                queryParams["genre"] = genre;

            if (!string.IsNullOrEmpty(keyword))
                queryParams["keyword"] = keyword;

            if (!string.IsNullOrEmpty(companie))
                queryParams["companie"] = companie;

            if (!string.IsNullOrEmpty(language))
                queryParams["language"] = language;

            if (!string.IsNullOrEmpty(theme))
                queryParams["theme"] = theme;

            if (!string.IsNullOrEmpty(releaseyear))
                queryParams["releaseyear"] = releaseyear;


            var query = queryParams.BuildSearchQuery(50);

            System.Console.WriteLine(query);

            var content = new StringContent(query);

            // Define o tipo de mídia do conteúdo como texto simples
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            httpClient.DefaultRequestHeaders.Add("Client-ID", "7xvfbiwcpi3xzgmwclqxumlsxg7py3");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer yygovoe0gg64fmec4igxsb9tzcjtg7");

            var response = await httpClient.PostAsync("https://api.igdb.com/v4/games", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(responseString);
                var responseContent = JsonConvert.DeserializeObject<List<SearchGameResponseModel>>(responseString);
                return responseContent.Ok();
            }
            else
            {
                throw new Exception($"Erro na requisição: {response.StatusCode}");
            }
        }
    }
}
