
using Domain.Enums;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchGame
{
    #region ParentObject
    public class GetGameByIdResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("artworks")]
        private List<Artwork> Artworks { get; set; }

        public List<string> ArtworkUrls => Artworks?.Select(prop => prop.ImageId ?? "").ToList();

        [JsonProperty("category")]
        private int _Category { get; set; }
        public string Category => Enum.GetName(typeof(GameCategory), _Category);

        [JsonProperty("collection")]
        public Collection Collection { get; set; }

        [JsonProperty("cover")]
        private Cover _Cover { get; set; }
        public string Cover => _Cover.Image_Id;

        [JsonProperty("first_release_date")]
        public int FirstReleaseDate { get; set; }

        [JsonProperty("genres")]
        private List<Genre> _Genres { get; set; }
        public List<string> Genres => _Genres?.Select(prop => prop.Name ?? "").ToList();

        [JsonProperty("involved_companies")]
        public List<InvolvedCompany> InvolvedCompanies { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }

        [JsonProperty("screenshots")]
        private List<Screenshot> _Screenshots { get; set; }
        public List<string> Screenshots => _Screenshots?.Select(prop => prop.ImageId ?? "").ToList();

        [JsonProperty("storyline")]
        public string Storyline { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("themes")]
        private List<Theme> _Themes { get; set; }
        public List<string> Themes => _Themes?.Select(prop => prop.Name ?? "").ToList();

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("videos")]
        public List<Video> Videos { get; set; }

        [JsonProperty("websites")]
        private List<Website> _Websites { get; set; }
        public List<string> Websites => _Websites?.Select(prop => prop.Url ?? "").ToList();
    }

    public class GameInfo
    {
        [JsonProperty("id")]
        public int GameId { get; set; }

        [JsonProperty("first_release_date")]
        public int FirstReleaseDate { get; set; }

        [JsonProperty("genres")]
        private List<Genre> _Genres { get; set; }
        public List<string> Genres => _Genres?.Select(prop => prop.Name).ToList();

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }

        [JsonProperty("storyline")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("cover")]
        private Cover _Cover { get; set; }
        public string Cover => _Cover.Image_Id ?? "";
    }

    #endregion

    #region Child Objects
    public struct Artwork
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }
    }

    public struct Collection
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("games")]
        public List<Game> Games { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public struct Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public struct Game
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public struct Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public struct InvolvedCompany
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("company")]
        public Company Company { get; set; }

        [JsonProperty("porting")]
        public bool Porting { get; set; }

        [JsonProperty("publisher")]
        public bool Publisher { get; set; }
    }

    public struct Platform
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform_logo")]
        private PlatformLogo _PlatformLogo { get; set; }
        public string PlatformLogo => _PlatformLogo.ImageId ?? "";
    }

    public struct PlatformLogo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }
    }
    public struct Screenshot
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }
    }

    public struct Theme
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public struct Video
    {
        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("video_id")]
        public string VideoId { get; set; }
    }

    public struct Website
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    #endregion
}
