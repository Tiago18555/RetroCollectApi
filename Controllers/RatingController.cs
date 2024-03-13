using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.GameOperations.AddRating;
using RetroCollectApi.Application.UseCases.GameOperations.ManageRating;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IAddRatingService addRating { get; set; }
        private IManageRatingService manageRating { get; set; }
        public RatingController(IAddRatingService addRating, IManageRatingService manageRating)
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
        public IActionResult AddRating([FromBody] AddRatingRequestModel request)
        {
            var result = addRating.AddRating(request);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPut("")]
        [SwaggerOperation(
            Summary = "Edit rating",
            Description = "Edit one or more info os rating"
        )]
        [SwaggerResponse(200, "Rating updated")]
        [SwaggerResponse(400, "Resource not found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult EditRating()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("")]
        [SwaggerOperation(
            Summary = "Delete rating",
            Description = "Deletes a rating register"
        )]
        [SwaggerResponse(200, "Rating deleted")]
        [SwaggerResponse(400, "Resource not found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteRating()
        {
            throw new NotImplementedException();
        }

    }
}

