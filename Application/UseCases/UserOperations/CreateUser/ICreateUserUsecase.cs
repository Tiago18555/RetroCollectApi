using Domain;

namespace Application.UseCases.UserOperations.CreateUser
{
    public interface ICreateUserUsecase
    {
        Task<ResponseModel> CreateUser(CreateUserRequestModel createUserRequestModel);
    }
}