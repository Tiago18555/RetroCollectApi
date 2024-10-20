using Application.UseCases.UserOperations.Authenticate;
using Application.UseCases.UserOperations.VerifyAndRecoverUser;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IAuthenticateUsecase Authenticate { get; set; }
    private IVerifyAndRecoverUserUsecase Verify { get; set; }

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
    public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestModel emailDto, CancellationToken cts)
    {
        var result = await Verify.SendEmail(emailDto, cts);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("recover/{username}/{timestamp_hash}")]
    [SwaggerOperation(
        Summary = "Change password page",
        Description = "Get changing-password page"
    )]
    [SwaggerResponse(404, "User not found")]
    [SwaggerResponse(500, "Internal server error")]
    public IActionResult ChangePasswordPage([FromRoute] string username, [FromRoute] string timestamp_hash)
    {
        var result = Verify.ChangePasswordTemplate(username, timestamp_hash);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return Content(result.Data as string, "text/html");
    }

    [HttpPatch("update/{username}/{timestamp_hash}")]
    [SwaggerOperation(
        Summary = "Change password",
        Description = "Update user password"
    )]
    [SwaggerResponse(404, "User not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> ChangePassword([FromRoute] string username, [FromBody] UpdatePasswordRequestModel pwd, [FromRoute] string timestamp_hash)
    {
        var result = await Verify.ChangePassword(username, pwd, timestamp_hash);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }

    [HttpGet("verify/{username}")]
    public async Task<IActionResult> ValidateUser([FromRoute] string username)
    {
        var result = await Verify.VerifyUser(username);

        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = result.Message;
        Response.StatusCode = result.StatusCode;
        return new ObjectResult(result);
    }
}