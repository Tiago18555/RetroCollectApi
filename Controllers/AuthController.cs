using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using Swashbuckle.AspNetCore.Annotations;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthenticateService Authenticate { get; set; }
        public AuthController(IAuthenticateService authenticate)
        {
            this.Authenticate = authenticate;
        }

        [HttpPost("validate")]
        [SwaggerOperation(Summary = "Check the credentials and if is not expired")]
        public ObjectResult ValidateJwtToken()
        {
            var JwtRes = Authenticate.ValidateJwtToken(Request.Headers["Authorization"]);

            Response.StatusCode = JwtRes.StatusCode;
            return new ObjectResult(JwtRes);

        }
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Check the credentials")]
        public IActionResult Login([FromBody] AuthenticateServiceRequestModel credentials)
        {
            var result = Authenticate.Login(credentials);

            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}
