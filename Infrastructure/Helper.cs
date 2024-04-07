using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure
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

            string baseHost = config.GetSection("Igdb:BaseUrl").Value;
            string clientId = config.GetSection("Igdb:Client-ID").Value;
            string token = "Bearer " + config.GetSection("Igdb:Token").Value;

            httpClient.DefaultRequestHeaders.Add("Client-ID", clientId);
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Client-ID: {clientId}");
            Console.WriteLine($"Token: {token.Substring(0,5)}...");
            Console.ForegroundColor = ConsoleColor.White;

            await Console.Out.WriteLineAsync($"URL: {baseHost}{endpoint}");

            var response = await httpClient.PostAsync($"{baseHost}{endpoint}", content);

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
