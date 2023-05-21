using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations;
using RetroCollectApi.Application.UseCases.UserOperations.CreateUser;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ICreateUserService CreateUserService { get; set; }


        public UserController(ICreateUserService createUser)
        {
            this.CreateUserService = createUser;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Registers a new user in the system."
        )]
        [SwaggerResponse(200, "User created successfully", typeof(CreateUserResponseModel))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(409, "Username or Email is already registered")]
        [SwaggerResponse(500, "Internal server error")]
        public ObjectResult CreateUser([FromBody] CreateUserRequestModel user)
        {
            var result = CreateUserService.CreateUser(user);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
        
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        ISearchGameService SearchGameService { get; set; }
        public GameController(ISearchGameService searchGameService)
        {
            SearchGameService = searchGameService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Search games",
            Description = "Search games from IGDB Database"
        )]
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
            var result = await SearchGameService.SearchBy(search, genre, keyword, companie, language, theme, releaseyear);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

    }
}

