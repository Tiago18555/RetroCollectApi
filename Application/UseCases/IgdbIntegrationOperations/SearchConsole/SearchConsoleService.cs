using Newtonsoft.Json;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
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
        public async Task<ResponseModel> SearchBy(string name)
        {
            if (string.IsNullOrEmpty(name)) { return GenericResponses.NotFound("Field \"search cannot be empty\""); }

            string query = $"fields name, platform_logo.image_id, versions.platform_version_release_dates.y; limit 50; where category = (1, 5); search \"{name.CleanKeyword()}\"; ";

            Console.WriteLine(query);

            var content = new StringContent(query);

            // Define o tipo de mídia do conteúdo como texto simples
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            httpClient.DefaultRequestHeaders.Add("Client-ID", "7xvfbiwcpi3xzgmwclqxumlsxg7py3");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer yygovoe0gg64fmec4igxsb9tzcjtg7");

            var response = await httpClient.PostAsync("https://api.igdb.com/v4/platforms", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
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
