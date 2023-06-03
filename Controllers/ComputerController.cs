using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private ISearchComputerService searchComputerService { get; set; }

        public ComputerController(ISearchComputerService _searchComputerService)
        {
            this.searchComputerService = _searchComputerService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Search computers",
            Description = "Search computers or arcades from IGDB Database"
        )]
        [SwaggerResponse(200, "Search completed")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> SearchComputer([FromQuery] string search)
        {
            var result = await searchComputerService.SearchBy(search);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get computer by id",
            Description = "Get a specific computer or arcade by your id from IGDB Database"
        )]
        [SwaggerResponse(200, "Search completed")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GetComputerById([FromRoute] int id)
        {
            var result = await searchComputerService.GetById(id);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}

