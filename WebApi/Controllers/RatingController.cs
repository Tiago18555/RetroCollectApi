using Application.UseCases.RatingOperations.AddRating;
using Application.UseCases.RatingOperations.ManageRating;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("rating")]
[ApiController]
public class RatingController : ControllerBase
{
    private readonly IAddRatingUsecase addRating;
    private readonly IManageRatingUsecase manageRating;
    public RatingController(IAddRatingUsecase addRating, IManageRatingUsecase manageRating)
    {
        this.addRating = addRating;
        this.manageRating = manageRating;
    }


    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Add new game rating",
        Description = "Adds a new rating / user review on a game"
    )]
    [SwaggerResponse(201, "Rating created")]
    [SwaggerResponse(400, "Resource not found")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> AddRating([FromBody] AddRatingRequestModel request)
    {
        var result = await addRating.AddRating(request, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpPut("")]
    [SwaggerOperation(
        Summary = "Edit rating",
        Description = "Edit one or more info of a rating"
    )]
    [SwaggerResponse(200, "Rating updated")]
    [SwaggerResponse(400, "Resource not found")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> EditRating([FromBody] EditRatingRequestModel request)
    {
        var result = await manageRating.EditRating(request, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpDelete("{rating_id}")]
    [SwaggerOperation(
        Summary = "Delete rating",
        Description = "Deletes a rating register"
    )]
    [SwaggerResponse(200, "Rating deleted")]
    [SwaggerResponse(400, "Resource not found")]
    [SwaggerResponse(406, "Invalid format of request")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid rating_id)
    {
        var result = await manageRating.RemoveRating(rating_id, HttpContext.User);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("user")]
    [SwaggerOperation(
        Summary = "List ratings by user",
        Description = "List all ratings made by specific user"
    )]
    [SwaggerResponse(200, "Ratings found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetAllRatingsByUser([FromQuery] int page_size, [FromQuery] int page_number)
    {
        var result = await manageRating.GetAllRatingsByUser(HttpContext.User, page_size, page_number);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("game/{id}")]
    [SwaggerOperation(
        Summary = "List ratings by game",
        Description = "List all ratings from a specific game"
    )]
    [SwaggerResponse(200, "Ratings found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetAllRatingsByGame([FromRoute] int id, [FromQuery] int page_size, [FromQuery] int page_number)
    {
        var result = await manageRating.GetAllRatingsByGame(id, page_size, page_number);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

}

