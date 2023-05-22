
using Newtonsoft.Json;

namespace RetroCollectApi.Application.UseCases.IgdbIntegrationOperations
{
    public class SearchGameResponseModel
    {
        public Cover Cover { get; set; }
        public string Name { get; set; }

        [JsonProperty("first_release_date")]
        public string ReleaseDate { get; set; }

    }
}
