using Domain;

namespace Application.UseCases.UserOperations.Authenticate
{
    public interface IAuthenticateUsecase
    {
        ResponseModel Login(AuthenticateServiceRequestModel credentials);
        ResponseModel ValidateJwtToken(string token);
    }
}
