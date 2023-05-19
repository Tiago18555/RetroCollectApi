using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.CreateUser
{
    public interface ICreateUserService
    {
        ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel);
    }
}