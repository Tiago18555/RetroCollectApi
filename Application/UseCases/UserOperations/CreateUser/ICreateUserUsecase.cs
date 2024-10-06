using CrossCutting;

namespace Application.UseCases.UserOperations.CreateUser
{
    public interface ICreateUserUsecase
    {
        ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel);
    }
}