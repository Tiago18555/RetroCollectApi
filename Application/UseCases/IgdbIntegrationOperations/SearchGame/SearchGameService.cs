using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame
{
    public class SearchGameService : ISearchGameService
    {
        private readonly HttpClient httpClient;

        public SearchGameService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            String query = @"
                fields artworks.image_id, 
                category, 
                collection.name, collection.games.name, 
                first_release_date, 
                genres.name, 
                involved_companies.porting, involved_companies.publisher, involved_companies.company.name, 
                name, 
                platforms.name, platforms.platform_logo.image_id, 
                screenshots.image_id, 
                storyline, 
                summary, 
                themes.name, 
                url, 
                videos.name, videos.video_id,
                websites.url; 
                where id = " + id.ToString() + ";";

            var res = await httpClient.IgdbPostAsync<List<GetGameByIdResponseModel>>(query, "games");

            return res.Ok();
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

            Console.WriteLine(query);

            var res = await httpClient.IgdbPostAsync<List<SearchGameResponseModel>>(query, "games");

            return res.Ok();

        }
    }
}
