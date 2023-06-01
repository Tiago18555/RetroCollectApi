using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserComputerController : ControllerBase
    {
        private IManageComputerCollectionService manageComputerCollectionService { get; set; }
        public UserComputerController(IManageComputerCollectionService manageComputerCollectionService)
        {
            this.manageComputerCollectionService = manageComputerCollectionService;
        }

        [HttpGet("{userId}")]
        [SwaggerOperation(
            Summary = "List computer collection",
            Description = "List all computers for a specified user"
        )]
        [SwaggerResponse(200, "List found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GetAllComputerByUser([FromRoute] Guid userId)
        {
            var result = await manageComputerCollectionService.GetAllComputersByUser(userId, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add computer",
            Description = "Adds specified computer to the specified user collection, and register this item if is not found on database"
        )]
        [SwaggerResponse(201, "Item added to collection")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> AddComputer([FromBody] AddItemRequestModel item)
        {
            var result = await manageComputerCollectionService.AddComputer(item, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpDelete("{userComputerId}")]
        [SwaggerOperation(
            Summary = "Delete computer",
            Description = "Delete computer of a specified user collection"
        )]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteComputer([FromRoute] Guid userComputerId)
        {
            var result = manageComputerCollectionService.DeleteComputer(userComputerId, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update computer",
            Description = "Update computer of a specified user collection"
        )]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> EditComputer([FromBody] UpdateComputerRequestModel computer)
        {
            var result = await manageComputerCollectionService.UpdateComputer(computer, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}

