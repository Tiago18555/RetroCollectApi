using Application.UseCases.IgdbIntegrationOperations.Shared;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchComputer;

public struct SearchComputerResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("platform_logo")]
    private Platform_Logo Platform_Logo { get; set; }

    public string LogoUrl => this.Platform_Logo.Image_Id;
}
