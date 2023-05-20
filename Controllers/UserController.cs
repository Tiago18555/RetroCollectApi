using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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
}

