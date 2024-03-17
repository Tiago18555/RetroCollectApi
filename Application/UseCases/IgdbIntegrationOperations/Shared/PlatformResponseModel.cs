using Domain.Enums;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.Shared
{
    public class PlatformResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("alternative_name")]
        public string AlternativeName { get; set; }

        [JsonProperty("category")]
        private int _Category { get; set; }
        public string Category => Enum.GetName(typeof(PlatformCategory), _Category);

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform_logo")]
        private PlatformLogo _PlatformLogo { get; set; }
        public string PlatformLogo => _PlatformLogo.ImageId ?? "";

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("versions")]
        public List<Version> Versions { get; set; }

        [JsonProperty("websites")]
        public List<Website> Websites { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }


    public struct PlatformLogo
    {
        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image_id")]
        public string ImageId { get; set; }
    }

    public struct Version
    {
        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform_logo")]
        public PlatformLogo PlatformLogo { get; set; }
    }

    public struct Website
    {
        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("category")]
        private int _Category { get; set; }
        public string Category => Enum.GetName(typeof(PlatformWebsite), _Category);
    }
}
