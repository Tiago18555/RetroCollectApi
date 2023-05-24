using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace RetroCollectApi.Application.UseCases.UserOperations.CreateUser
{
    public class CreateUserService : ICreateUserService
    {
        private IUserRepository repository { get; set; }
        public CreateUserService(IUserRepository _repository)
        {
            this.repository = _repository;
        }

        public ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel)
        {
            User user = createUserRequestModel.MapObjectTo(new User());

            if (repository.Any(x => x.Username == createUserRequestModel.Username || x.Email == createUserRequestModel.Email))
            {
                return GenericResponses.Conflict();
            }

            user.Password = BCryptNet.HashPassword(createUserRequestModel.Password);
            user.CreatedAt = DateTime.Now;

            try
            {
                return this.repository.Add(user)
                    .MapObjectTo(new CreateUserResponseModel())
                    .Created();
            }
            catch (DBConcurrencyException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (InvalidOperationException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
