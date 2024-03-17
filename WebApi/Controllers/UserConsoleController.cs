using Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using Application.UseCases.UserCollectionOperations.Shared;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserConsoleController : ControllerBase
    {
        private IManageConsoleCollectionService manageConsoleCollectionService { get; set; }
        public UserConsoleController(IManageConsoleCollectionService manageConsoleCollectionService)
        {
            this.manageConsoleCollectionService = manageConsoleCollectionService;
        }

        [HttpGet("{user_id}")]
        [SwaggerOperation(
            Summary = "List console collection",
            Description = "List all consoles for a specified user"
        )]
        [SwaggerResponse(200, "List found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GetAllConsoleByUser([FromRoute] Guid user_id)
        {
            var result = await manageConsoleCollectionService.GetAllConsolesByUser(HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add console",
            Description = "Adds specified console to the specified user collection, and register this item if is not found on database"
        )]
        [SwaggerResponse(201, "Item added to collection")]
        [SwaggerResponse(400, "Invalid format of request")]
        [SwaggerResponse(403, "Invalid credentials")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(415, "Unsupported media type")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> AddConsole([FromBody] AddItemRequestModel item)
        {
            var result = await manageConsoleCollectionService.AddConsole(item, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpDelete("{user_console_id}")]
        [SwaggerOperation(
            Summary = "Delete console",
            Description = "Delete console of a specified user collection"
        )]
        [SwaggerResponse(200, "Deleted successfully")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteConsole([FromRoute] Guid user_console_id)
        {
            var result = manageConsoleCollectionService.DeleteConsole(user_console_id, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update console",
            Description = "Update console of a specified user collection"
        )]
        [SwaggerResponse(200, "Update successfully")]
        [SwaggerResponse(400, "Invalid format of request")]
        [SwaggerResponse(403, "Invalid credentials")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> EditConsole([FromBody] UpdateConsoleRequestModel console)
        {
            var result = await manageConsoleCollectionService.UpdateConsole(console, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}

