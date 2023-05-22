using Newtonsoft.Json;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer
{
    public struct SearchComputerResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform_logo")]
        private Platform_Logo Platform_Logo { get; set; }

        public string LogoUrl => this.Platform_Logo.Image_Id;
        public int ReleaseYear => this.Versions.Pvrd.y;

        [JsonProperty("versions")]
        private Versions Versions { get; set; }
    }
}
