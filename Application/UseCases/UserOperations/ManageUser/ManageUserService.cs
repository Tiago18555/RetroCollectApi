using CrossCutting;
using CrossCutting.Providers;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Application.UseCases.UserOperations.ManageUser
{
    public class ManageUserService : IManageUserService
    {
        private readonly IUserRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        public ManageUserService(IUserRepository repository, IDateTimeProvider dateTimeProvider)
        {
            this._repository = repository;
            this._dateTimeProvider = dateTimeProvider;
        }

        public ResponseModel UpdateUser(UpdateUserRequestModel userRequestModel, ClaimsPrincipal user)
        {
            try
            {
                if (!user.IsTheRequestedOneId(userRequestModel.UserId)) return GenericResponses.Forbidden();
                var foundUser = _repository.SingleOrDefault(x => x.UserId == userRequestModel.UserId);

                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var res = this._repository.Update(foundUser.MapAndFill(userRequestModel, _dateTimeProvider));

                return res
                    .MapObjectTo( new UpdateUserResponseModel() )
                    .Ok();
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
            var res = await _repository.GetAll(x => x);
            return res.Ok();
        }
    }

}
