
using Newtonsoft.Json;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations
{
    public partial class SearchGameResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cover")]
        public Cover Cover { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("keywords")]
        public List<Keyword> Keywords { get; set; }

        [JsonProperty("platforms")]
        public List<Platform> Platforms { get; set; }

        [JsonProperty("themes")]
        public List<Theme> Themes { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("storyline")]
        public string Storyline { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }
    }

    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Keyword
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Platform
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Theme
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Cover
    {
        [JsonProperty("image_id")]
        public string Image_Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
