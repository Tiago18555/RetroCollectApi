
using Newtonsoft.Json;

namespace Application.UseCases.IgdbIntegrationOperations.SearchGame;

public class SearchGameResponseModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("cover")]
    private Cover Cover { get; set; }
    public string CoverUrl => Cover.Image_Id;
    public string Name { get; set; }

    [JsonProperty("first_release_date")]
    public string ReleaseDate { get; set; }
}

public struct Cover
{
    [JsonIgnore]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("image_id")]
    public string Image_Id { get; set; }
}
