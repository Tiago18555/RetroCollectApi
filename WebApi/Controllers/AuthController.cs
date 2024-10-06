using Application.UseCases.UserOperations.Authenticate;
using Application.UseCases.UserOperations.VerifyAndRecoverUser;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Application.UseCases.UserOperations.VerifyAndRecoverUser.VerifyAndRecoverUserUsecase;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthenticateUsecase Authenticate { get; set; }
        IVerifyAndRecoverUserUsecase Verify { get; set; }

        public AuthController(IAuthenticateUsecase authenticate, IVerifyAndRecoverUserUsecase verify)
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

        [HttpGet("recover/{user_id}/{timestamp_hash}")]
        [SwaggerOperation(
            Summary = "Change password page",
            Description = "Get changing-password page"
        )]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult ChangePasswordPage([FromRoute] Guid user_id, [FromRoute] string timestamp_hash)
        {
            var result = Verify.ChangePasswordTemplate(user_id, timestamp_hash);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return Content(result.Data as string, "text/html");
        }

        [HttpPatch("update/{user_id}/{timestamp_hash}")]
        [SwaggerOperation(
            Summary = "Change password",
            Description = "Update user password"
        )]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error")]
        public IActionResult ChangePassword([FromRoute] Guid user_id, [FromBody] UpdatePasswordRequestModel pwd, [FromRoute] string timestamp_hash)
        {
            var result = Verify.ChangePassword(user_id, pwd, timestamp_hash);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }

        [HttpGet("verify/{user_id}")]
        public IActionResult ValidateUser([FromRoute] Guid user_id)
        {
            var result = Verify.VerifyUser(user_id);

            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
            Response.StatusCode = result.StatusCode;
            return new ObjectResult(result);
        }
    }
}
