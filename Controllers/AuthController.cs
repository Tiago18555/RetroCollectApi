using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser;
using RetroCollectApi.CrossCutting;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using static RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser.VerifyAndRecoverUserService;

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
        [SwaggerOperation(
            Summary = "Send email",
            Description = "Send email with instructions and a link for redirect to ChangePassword end-point. Only email or username are allowed on this request, not both"
        )]
        [SwaggerResponse(200, "Email sent")]
        [SwaggerResponse(400, "Invalid format of request")]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult SendEmail([FromBody] SendEmailRequestModel emailDto)
        {
            var result = Verify.SendEmail(emailDto);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpGet("recover/{userid}")]
        [SwaggerOperation(
            Summary = "Change password page",
            Description = "Get changing-password page"
        )]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult ChangePasswordPage([FromRoute] Guid userid)
        {
            var result = Verify.ChangePasswordTemplate(userid);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return Content(result.Data as string, "text/html");
        }

        [HttpPatch("update/{userid}")]
        [SwaggerOperation(
            Summary = "Change password",
            Description = "Update user password"
        )]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult ChangePassword([FromRoute] Guid userid, [FromBody] UpdatePasswordRequestModel pwd)
        {
            var result = Verify.ChangePassword(userid, pwd);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}
