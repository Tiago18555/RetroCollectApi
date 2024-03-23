using CrossCutting;

namespace Application.UseCases.UserOperations.Authenticate
{
    public interface IAuthenticateService
    {
        ResponseModel Login(AuthenticateServiceRequestModel credentials);
        ResponseModel ValidateJwtToken(string token);
    }
}
