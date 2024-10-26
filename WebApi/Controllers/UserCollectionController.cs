using Application.UseCases.UserCollectionOperations.ManageGameCollection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("usercollection")]
[ApiController]
public class UserCollectionController : ControllerBase
{
    private readonly IManageGameCollectionUsecase manageGameCollectionService;
    public UserCollectionController(IManageGameCollectionUsecase manageGameCollectionService)
    {
        this.manageGameCollectionService = manageGameCollectionService;
    }

    [HttpGet("{user_id}")]
    [SwaggerOperation(
        Summary = "List game collection",
        Description = "List all game collection for a specified user"
    )]
    [SwaggerResponse(200, "List found")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetAllGamesByUser([FromRoute] Guid user_id)
    {
        var result = await manageGameCollectionService.GetAllGamesByUser(HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add game",
        Description = "Adds specified game to the specified user collection, and register this game if is not found on database \n" +
                      "Propertie Condition can be { new, likenew, used, fair, and poor } \n" +
                      "Propertie Ownership Status can be { owned, desired, traded, borrowed and sold }"
    )]
    [SwaggerResponse(201, "Item added to collection")]
    [SwaggerResponse(400, "Invalid format of request")]
    [SwaggerResponse(403, "Invalid credentials")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(415, "Unsupported media type")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> AddGame([FromBody] AddGameRequestModel item)
    {
        var result = await manageGameCollectionService.AddGame(item, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }


    [HttpDelete("{user_collection_id}")]
    [SwaggerOperation(
        Summary = "Delete game",
        Description = "Delete game of a specified user collection"
    )]
    [SwaggerResponse(200, "Deleted successfully")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> DeleteGame([FromRoute] Guid user_collection_id)
    {
        var result = await manageGameCollectionService.DeleteGame(user_collection_id, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Update game",
        Description = "Update game of a specified user collection"
    )]
    [SwaggerResponse(200, "Updated successfully")]
    [SwaggerResponse(400, "Invalid format of request")]
    [SwaggerResponse(403, "Invalid credentials")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> EditGame([FromBody] UpdateGameRequestModel updateGameRequestModel)
    {
        var result = await manageGameCollectionService.UpdateGame(updateGameRequestModel, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }
}

