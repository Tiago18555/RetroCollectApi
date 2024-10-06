using CrossCutting;
using Infrastructure;
using Application.UseCases.IgdbIntegrationOperations.Shared;

namespace Application.UseCases.IgdbIntegrationOperations.SearchGame
{
    public class SearchGameUsecase : ISearchGameUsecase
    {
        private readonly HttpClient _httpClient;

        public SearchGameUsecase()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ResponseModel> GetById(int id)
        {
            String query = @"
                fields artworks.image_id, 
                category, 
                collection.name, collection.games.name,
                cover.image_id,
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

            var res = await _httpClient.IgdbPostAsync<List<GetGameByIdResponseModel>>(query, "games");

            return res.Ok();
        }

        public async Task<List<GameInfo>> RetrieveGameInfoAsync(int game_id)
        {
            String query = @"
                fields
                first_release_date, 
                genres.name,  
                name, 
                platforms.name, platforms.platform_logo.image_id,
                storyline, 
                summary,
                cover.image_id; 
                where id = " + game_id.ToString() + ";";

            var res = await _httpClient.IgdbPostAsync<List<GameInfo>>(query, "games");

            return res;
        }

        public async Task<ResponseModel> SearchBy(string search, string genre, string keyword, string companie, string language, string theme, string releaseyear, int limit)
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

            if (limit > 0 && limit < 500)
                queryParams["limit"] = limit.ToString();


            var query = queryParams.BuildSearchQuery(limit);

            Console.WriteLine(query);

            var res = await _httpClient.IgdbPostAsync<List<SearchGameResponseModel>>(query, "games");

            return res.Ok();

        }
    }
}
