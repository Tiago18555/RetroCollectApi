using CrossCutting;
using CrossCutting.Providers;
using Domain;
using Domain.Broker;
using Domain.Exceptions;
using Domain.Repositories;
using MongoDB.Driver.Core.Authentication;
using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace Application.UseCases.UserOperations.ManageUser;

public class ManageUserUsecase : IManageUserUsecase
{
    private readonly IUserRepository _repository;
    private readonly IProducerService _producer;
    public ManageUserUsecase(
        IUserRepository repository,
        IProducerService producer
    )
    {
        _repository = repository;
        _producer = producer;
    }

    public async Task<ResponseModel> UpdateUser(UpdateUserRequestModel request, ClaimsPrincipal requestClaim)
    {
        var userId = requestClaim.GetUserId();
        if (!_repository.Any(x => x.UserId == userId))
            return ResponseFactory.NotFound("User not found");

        try
        {   
            var messageObject = new MessageModel
            { 
                Message = new
                {
                    UserId = userId,
                    request.Username,
                    request.Email,
                    request.FirstName,
                    request.LastName                    
                }, SourceType = "update-user" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize(
                message, 
                typeof( MessageModel )
            ) as MessageModel;

            return "Success".Ok();
        }
        catch (ArgumentNullException)
        {
            return ResponseFactory.NotAcceptable("Formato de dados inválido");
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
