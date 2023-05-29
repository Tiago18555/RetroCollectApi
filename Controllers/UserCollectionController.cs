using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCollectionController : ControllerBase
    {
        private IManageGameCollectionService manageGameCollectionService { get; set; }
        public UserCollectionController(IManageGameCollectionService manageGameCollectionService)
        {
            this.manageGameCollectionService = manageGameCollectionService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add game",
            Description = "Adds specified game to the specified user collection, and register this game if is not found on database \n" +
                          "Propertie Condition can be { new, likenew, used, fair, and poor } \n" +
                          "Propertie Ownership Status can be { owned, desired, traded, borrowed and sold }"
        )]
        [SwaggerResponse(201, "Item added to collection")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> AddGame([FromBody] AddGameRequestModel item)
        {
            var result = await manageGameCollectionService.AddGame(item, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }


        [HttpDelete]
        [SwaggerOperation(
            Summary = "Delete game",
            Description = "Delete game of a specified user collection"
        )]
        [SwaggerResponse(201, "Item added to collection")]
        [SwaggerResponse(406, "Invalid format of request")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult DeleteGame([FromRoute] Guid id)
        {
            var result = manageGameCollectionService.DeleteGame(id, HttpContext.User);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}

