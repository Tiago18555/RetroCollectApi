using Domain;
using Domain.Broker;
using Domain.Exceptions;
using Domain.Repositories;
using CrossCutting;
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

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "user");

            var data = JsonSerializer.Deserialize(
                message, 
                typeof( MessageModel )
            ) as MessageModel;

            return "Success".Ok();
        }
        catch (ArgumentNullException err)
        {
            return ResponseFactory.NotAcceptable($"Formato de dados inválido {err.Message}");
        }
        catch (NullClaimException err)
        {
            return ResponseFactory.BadRequest(err.Message);
        }
        catch (Exception err)       
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
    }

    public async Task<ResponseModel> GetAllUsers()
    {
        var res = await _repository.GetAll(x => x);
        return res.Ok();
    }
}
