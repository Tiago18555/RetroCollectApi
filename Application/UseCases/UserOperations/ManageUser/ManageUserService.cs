using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public class ManageUserService : IManageUserService
    {
        private IUserRepository repository { get; set; }
        public ManageUserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public ResponseModel UpdateUser(UpdateUserRequestModel userRequestModel, ClaimsPrincipal user)
        {
            try
            {
                if (!user.IsTheRequestedOneId(userRequestModel.UserId)) return GenericResponses.Forbidden();
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
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.Message.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel> GetAllUsers()
        {
            var res = await repository.GetAll(x => x);
            return res.Ok();
        }
    }

}
