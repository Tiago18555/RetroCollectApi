using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories;
using System.Data;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public class ManageUserService : IManageUserService
    {
        private IUserRepository repository { get; set; }
        public ManageUserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public ResponseModel UpdateUser(UpdateUserRequestModel userRequestModel)
        {
            try
            {
                var foundUser = repository.SingleOrDefault(x => x.UserId == userRequestModel.UserId);

                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var user = foundUser.MapObjectTo(new User());

                if (userRequestModel.Username != null) { user.Username = userRequestModel.Username; }
                if (userRequestModel.Email != null) { user.Email = userRequestModel.Email; }
                if (userRequestModel.FirstName != null) { user.FirstName = userRequestModel.FirstName; }
                if (userRequestModel.LastName != null) { user.LastName = userRequestModel.LastName; }

                user.UpdatedAt = DateTime.Now;

                var res = this.repository.Update(user);

                return res.Ok();
            }
            catch (ArgumentNullException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DBConcurrencyException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DbUpdateException)
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

        public ResponseModel UpdatePassword(UpdatePwdRequestModel user)
        {
            throw new NotImplementedException();
        }
    }
}
