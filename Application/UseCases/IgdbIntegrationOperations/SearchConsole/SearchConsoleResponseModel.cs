using Application.UseCases.IgdbIntegrationOperations.Shared;
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchConsole;

public struct SearchConsoleResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("platform_logo")]
    private Platform_Logo Platform_Logo { get; set; }

    public string LogoUrl => Platform_Logo.Image_Id;
}
