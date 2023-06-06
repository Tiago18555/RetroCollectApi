using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RetroCollectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthenticateService Authenticate { get; set; }
        IVerifyAndRecoverUserService Verify { get; set; }

        public AuthController(IAuthenticateService authenticate, IVerifyAndRecoverUserService verify)
        {
            this.Authenticate = authenticate;
            this.Verify = verify;
        }

        [HttpPost("validate")]
        [SwaggerOperation(Summary = "Check the credentials and if is not expired")]
        public ObjectResult ValidateJwtToken()
        {
            var result = Authenticate.ValidateJwtToken(Request.Headers["Authorization"]);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);

        }
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Check the credentials")]
        public IActionResult Login([FromBody] AuthenticateServiceRequestModel credentials)
        {
            var result = Authenticate.Login(credentials);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] EmailDto emailDto)
        {
            Verify.SendEmail(emailDto);
            return Ok();

            //Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            //Response.StatusCode = result.StatusCode;
            //return new ObjectResult(result);
        }

        [HttpGet("recover/{userid}")]
        public IActionResult ChangePassword([FromRoute] Guid userid)
        {
            var res = Verify.ChangePasswordTemplate(userid);
            return Content(res, "text/html");
        }
    }
}
