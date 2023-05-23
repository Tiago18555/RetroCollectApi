using Newtonsoft.Json;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.Shared
{
    public struct Platform_Version_Release_Dates
    {
        [JsonProperty("y")]
        public int y { get; set; }
    }
}
