using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("game")]
[ApiController]
public class GameController : ControllerBase
{
    private ISearchGameUsecase searchGameService { get; set; }
    public GameController(ISearchGameUsecase _searchGameService)
    {
        this.searchGameService = _searchGameService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Search games",
        Description = "Search games from IGDB Database"
    )]
    [SwaggerResponse(200, "Search completed")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> SearchGame(
        [FromQuery, SwaggerParameter("The search term for the game")] string search,
        [FromQuery, SwaggerParameter("The genre of the game")] string genre,
        [FromQuery, SwaggerParameter("Keywords related to the game")] string keyword,
        [FromQuery, SwaggerParameter("The company that developed the game")] string companie,
        [FromQuery, SwaggerParameter("The language version of the game")] string language,
        [FromQuery, SwaggerParameter("The theme of the game")] string theme,
        [FromQuery, SwaggerParameter("The year that this game was released")] string releaseyear,
        [FromQuery, SwaggerParameter("The maximum number of results to return, if not specified the dafault value is 50")] int limit
    )
    {
        var result = await searchGameService.SearchBy(search, genre, keyword, companie, language, theme, releaseyear, limit);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get game by id",
        Description = "Get a specific game by your id from IGDB Database"
    )]
    [SwaggerResponse(200, "Search completed")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetGameById([FromRoute] int id)
    {
        var result = await searchGameService.GetById(id);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

}


