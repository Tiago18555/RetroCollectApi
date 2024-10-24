using Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using Application.UseCases.UserCollectionOperations.Shared;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("usercomputer")]
[ApiController]
public class UserComputerController : ControllerBase
{
    private IManageComputerCollectionUsecase manageComputerCollectionService { get; set; }
    public UserComputerController(IManageComputerCollectionUsecase manageComputerCollectionService)
    {
        this.manageComputerCollectionService = manageComputerCollectionService;
    }

    [HttpGet("{user_id}")]
    [SwaggerOperation(
        Summary = "List computer collection",
        Description = "List all computers for a specified user"
    )]
    [SwaggerResponse(200, "List found")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetAllComputerByUser([FromRoute] Guid user_id)
    {
        var result = await manageComputerCollectionService.GetAllComputersByUser(HttpContext.User);

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
    [SwaggerResponse(400, "Invalid format of request")]
    [SwaggerResponse(403, "Invalid credentials")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(415, "Unsupported media type")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> AddComputer([FromBody] AddItemRequestModel item)
    {
        var result = await manageComputerCollectionService.AddComputer(item, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpDelete("{user_computer_id}")]
    [SwaggerOperation(
        Summary = "Delete computer",
        Description = "Delete computer of a specified user collection"
    )]
    [SwaggerResponse(200, "Deleted successfully")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> DeleteComputer([FromRoute] Guid user_computer_id)
    {
        var result = await manageComputerCollectionService.DeleteComputer(user_computer_id, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Update computer",
        Description = "Update computer of a specified user collection"
    )]
    [SwaggerResponse(200, "Updated successfully")]
    [SwaggerResponse(400, "Invalid format of request")]
    [SwaggerResponse(403, "Invalid credentials")]
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

