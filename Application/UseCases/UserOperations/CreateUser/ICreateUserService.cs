using Application.CrossCutting;

namespace Application.UseCases.UserOperations.CreateUser
{
    public interface ICreateUserService
    {
        ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel);
    }
}