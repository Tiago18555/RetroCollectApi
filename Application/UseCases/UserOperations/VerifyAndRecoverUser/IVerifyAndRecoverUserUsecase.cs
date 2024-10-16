using Domain;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public interface IVerifyAndRecoverUserUsecase
{
    Task<ResponseModel> SendEmail(SendEmailRequestModel request);
    ResponseModel VerifyUser(Guid userid);
    ResponseModel ChangePasswordTemplate(Guid userid, string timestamphash);
    Task<ResponseModel> ChangePassword(Guid user_id, UpdatePasswordRequestModel pwd, string timestamphash);
}

