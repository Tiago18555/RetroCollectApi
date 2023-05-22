using Newtonsoft.Json;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared
{
    public struct Versions
    {
        [JsonProperty("platform_version_release_dates")]
        public Platform_Version_Release_Dates Pvrd { get; set; }
    }
}
