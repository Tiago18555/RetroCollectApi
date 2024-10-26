using Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("console")]
[ApiController]
public class ConsoleController : ControllerBase
{
    private readonly ISearchConsoleUsecase searchConsoleService;

    public ConsoleController(ISearchConsoleUsecase _searchConsoleService)
    {
        this.searchConsoleService = _searchConsoleService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Search consoles",
        Description = "Search consoles from IGDB Database"
    )]
    [SwaggerResponse(200, "Search completed")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> SearchConsole(
        [FromQuery, SwaggerParameter("The search term for the game")] string search, 
        [FromQuery, SwaggerParameter("The maximum number of results to return, if not specified the dafault value is 50")] int limit
    )
    {
        var result = await searchConsoleService.SearchBy(search, limit);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get console by id",
        Description = "Get a specific console by your id from IGDB Database"
    )]
    [SwaggerResponse(200, "Search completed")]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetConsoleById([FromRoute] int id)
    {
        var result = await searchConsoleService.GetById(id);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }
}


