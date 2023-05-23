using Newtonsoft.Json;
using System.Drawing;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared
{
    public static class Helper
    {
        public static async Task<T> IgdbPostAsync<T>(this HttpClient httpClient, string query, string endpoint)
        {
            var content = new StringContent(query);

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            httpClient.DefaultRequestHeaders.Add("Client-ID", "7xvfbiwcpi3xzgmwclqxumlsxg7py3");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer yygovoe0gg64fmec4igxsb9tzcjtg7");

            var response = await httpClient.PostAsync($"https://api.igdb.com/v4/{endpoint}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("RESPONSE:");
                Console.WriteLine(responseString);
                Console.ForegroundColor = ConsoleColor.White;
                var responseContent = JsonConvert.DeserializeObject<T>(responseString);
                return responseContent;
            }
            else
            {
                throw new Exception($"Erro na requisição: {response.StatusCode}");
            }
        }
    }
}
