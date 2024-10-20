using Domain;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public interface IVerifyAndRecoverUserUsecase
{
    Task<ResponseModel> SendEmail(SendEmailRequestModel request, CancellationToken cts);
    Task<ResponseModel> VerifyUser(string username);
    ResponseModel ChangePasswordTemplate(string username, string timestamphash);
    Task<ResponseModel> ChangePassword(string username, UpdatePasswordRequestModel pwd, string timestamphash);
}

