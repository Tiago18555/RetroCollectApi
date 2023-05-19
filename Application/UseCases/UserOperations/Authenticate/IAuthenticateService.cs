using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.Authenticate
{
    public interface IAuthenticateService
    {
        ResponseModel Login(AuthenticateServiceRequestModel credentials);
        ResponseModel ValidateJwtToken(string token);
    }
}
