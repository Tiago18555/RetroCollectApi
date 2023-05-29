using Microsoft.EntityFrameworkCore;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories.Interfaces;
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

                var res = this.repository.Update(foundUser.MapAndFill(userRequestModel));

                return res.Ok();
            }
            catch (ArgumentNullException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DBConcurrencyException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DbUpdateException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (InvalidOperationException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido.");
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
