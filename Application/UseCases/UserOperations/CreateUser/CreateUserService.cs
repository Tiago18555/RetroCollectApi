using RetroCollect.Data;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using BCryptNet = BCrypt.Net.BCrypt;

namespace RetroCollectApi.Application.UseCases.UserOperations.CreateUser
{
    public class CreateUserService : ICreateUserService
    {
        private DataContext database { get; set; }
        public CreateUserService(DataContext dataContext)
        {
            this.database = dataContext;
        }

        public ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel)
        {
            User user = createUserRequestModel.MapObjectTo(new User());

            if(database.Users.Any(x => x.Username == user.Username))
            {
                return GenericResponses.Conflict();
            }

            user.Password = BCryptNet.HashPassword(createUserRequestModel.Password);
            user.CreatedAt = DateTime.Now;

            try
            {
                database.Users.Add(user);
                database.SaveChanges();
                database.Entry(user).GetDatabaseValues();

                return user
                        .MapObjectTo(new CreateUserResponseModel())
                        .Created();
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
