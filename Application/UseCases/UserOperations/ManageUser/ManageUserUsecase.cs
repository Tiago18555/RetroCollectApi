using CrossCutting;
using CrossCutting.Providers;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Application.UseCases.UserOperations.ManageUser;

public class ManageUserUsecase : IManageUserUsecase
{
    private readonly IUserRepository _repository;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ManageUserUsecase(IUserRepository repository, IDateTimeProvider dateTimeProvider)
    {
        this._repository = repository;
        this._dateTimeProvider = dateTimeProvider;
    }

    public ResponseModel UpdateUser(UpdateUserRequestModel userRequestModel, ClaimsPrincipal user)
    {
        try
        {
            if (!user.IsTheRequestedOneId(userRequestModel.UserId)) return ResponseFactory.Forbidden();
            var foundUser = _repository.SingleOrDefault(x => x.UserId == userRequestModel.UserId);

            if (foundUser == null) { return ResponseFactory.NotFound("User not found"); }

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
            return ResponseFactory.BadRequest(msg.Message.ToString());
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
