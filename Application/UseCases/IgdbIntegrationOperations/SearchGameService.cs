using Newtonsoft.Json;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations
{
    public interface ISearchGameService
    {
        Task<ResponseModel> SearchByTitle(SearchGameRequestModel searchGameRequestModel);
    }
    public class SearchGameService : ISearchGameService
    {
        private readonly HttpClient httpClient;

        public SearchGameService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> SearchByTitle(SearchGameRequestModel searchGameRequestModel)
        {
            if(searchGameRequestModel.Keyword.Length > 48)
            {
                return GenericResponses.BadRequest("Palavra chave não pode ser maior que 48 caracteres");
            }

            var keyword = searchGameRequestModel.Keyword.Trim().Replace("/", "").Replace("\"", "").Replace('\\', '/');


            var query = String.Format(
                $" fields cover.url, cover.image_id, genres.name, genres.url, " +
                $"keywords.name, keywords.url, name, platforms.name, platforms.url, " +
                $"rating, storyline, themes.name, themes.url, url; limit 5; search \"{keyword}\";"
            );
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
