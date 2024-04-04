using Application.UseCases.UserWishlistOperations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private IWishlistService wishlistService { get; set; }

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        [HttpPost("")]
        [SwaggerOperation(
            Summary = "Add game to wishlist",
            Description = "Adds a new game to user's wishlist"
        )]
        [SwaggerResponse(201, "Rating created")]
        [SwaggerResponse(400, "Resource not found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult AddWishlist([FromBody] AddToUserWishlistRequestModel request)
        {
            var result = wishlistService.Add(request, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        //public ResponseModel Add(AddToUserWishlistRequestModel RequestBody, ClaimsPrincipal RequestToken)

        [HttpDelete("{game_id}")]
        [SwaggerOperation(
            Summary = "Delete game from wishlist",
            Description = "Deletes a game from user's wishlist"
        )]
        [SwaggerResponse(200, "Wishlist deleted")]
        [SwaggerResponse(400, "Resource not found")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteWishlist([FromRoute] int game_id)
        {
            var result = wishlistService.Remove(game_id, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

    }
}

