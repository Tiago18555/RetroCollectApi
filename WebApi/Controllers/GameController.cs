using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private ISearchGameService searchGameService { get; set; }
        public GameController(ISearchGameService _searchGameService)
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
            [FromQuery] string search,
            [FromQuery] string genre,
            [FromQuery] string keyword,
            [FromQuery] string companie,
            [FromQuery] string language,
            [FromQuery] string theme,
            [FromQuery] string releaseyear
        )
        {
            var result = await searchGameService.SearchBy(search, genre, keyword, companie, language, theme, releaseyear);

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
}

