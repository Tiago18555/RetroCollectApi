using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared
{
    public static class Helper
    {
        public static async Task<T> IgdbPostAsync<T>(this HttpClient httpClient, string query, string endpoint)
        {
            await Console.Out.WriteLineAsync(AppContext.BaseDirectory);
            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("appsettings.Docker.json")
            .Build();

            var content = new StringContent(query);

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            httpClient.DefaultRequestHeaders.Add("Client-ID", config.GetSection("Igdb:Client-ID").Value);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + config.GetSection("Igdb:Token").Value);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Client-ID: {config.GetSection("Igdb:Client-ID").Value}");
            Console.WriteLine($"Token: {config.GetSection("Igdb:Token").Value.Substring(0,5)}...");
            Console.ForegroundColor = ConsoleColor.White;

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
                throw new Exception($"Erro na requisição: {response.ReasonPhrase}");
            }
        }
    }
}
