using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
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
        [SwaggerOperation(Summary = "Register a new user")]
        public ObjectResult CreateUser([FromBody] CreateUserRequestModel user)
        {
            var result = CreateUserService.CreateUser(user);

            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
        
    }
}

