using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
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

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add console",
            Description = "Adds specified console to the specified user collection, and register this item if is not found on database"
        )]
        [SwaggerResponse(201, "Item added to collection")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> AddConsole([FromBody] AddItemRequestModel item)
        {
            var result = await manageConsoleCollectionService.AddConsole(item, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Delete console",
            Description = "Delete console of a specified user collection"
        )]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteConsole([FromRoute] Guid id)
        {
            var result = manageConsoleCollectionService.DeleteConsole(id, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Update console",
            Description = "Update console of a specified user collection"
        )]
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

