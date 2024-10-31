using Domain;
using Application.UseCases.UserCollectionOperations.Shared;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;
using Domain.Repositories;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;

namespace Application.UseCases.UserCollectionOperations.ManageConsoleCollection;

public class ManageConsoleCollectionUsecase : IManageConsoleCollectionUsecase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserConsoleRepository _userConsoleRepository;
    private readonly IProducerService _producer;

    public ManageConsoleCollectionUsecase(
        IUserRepository userRepository, 
        IUserConsoleRepository userConsoleRepository, 
        IProducerService producer
    )
    {
        _userRepository = userRepository;
        _userConsoleRepository = userConsoleRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> AddConsole(AddItemRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        var user_id = requestToken.GetUserId();

        var user = _userRepository.Any(u => u.UserId == user_id);
        if (!user) { return ResponseFactory.NotFound("User not found"); }



        try
        {
            var messageObject = new MessageModel{ Message = new {
                requestBody.ItemId,
                requestBody.Condition,
                requestBody.Notes,
                requestBody.OwnershipStatus,
                requestBody.PurchaseDate
            }, SourceType = "add-console" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (InvalidEnumTypeException err)
        {
            return ResponseFactory.UnsupportedMediaType("Invalid type for Condition or OwnershipStatus: " + err.Message);
        }
        catch (InvalidEnumValueException err)
        {
            return ResponseFactory.BadRequest("Invalid value for Condition or OwnershipStatus: " + err.Message);
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
    public async Task<ResponseModel> DeleteConsole(Guid user_console_id, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var foundItem = _userConsoleRepository.SingleOrDefault(r => r.UserId == user_id && r.UserConsoleId == user_console_id);
            if (foundItem == null) { return ResponseFactory.NotFound(); }

            var messageObject = new MessageModel { Message = foundItem, SourceType = "delete-console" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Deleted successfully".Ok(message = status);
        }
        catch (ArgumentNullException err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
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

    public async Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel requestBody, ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId();

            var foundUser = _userRepository.Any(x => x.UserId == requestToken.GetUserId());
            if (!foundUser) { return ResponseFactory.NotFound("User not found"); }

            var foundConsole = _userConsoleRepository.Any(x => x.UserConsoleId == requestBody.UserConsoleId);
            if (!foundConsole) { return ResponseFactory.NotFound("Console Not Found"); }

            var messageObject = new MessageModel{ Message = new 
            {
                UserId = user_id,
                requestBody.UserConsoleId,
                requestBody.PurchaseDate,
                requestBody.Condition,
                requestBody.OwnershipStatus,
                requestBody.Notes
            }, 
            SourceType = "update-console" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Updated successfully".Ok(message = status);
        }
        catch (ArgumentNullException err)
        {
            return ResponseFactory.NotAcceptable($"Formato de dados inválido: {err.Message}");
        }
        catch (InvalidOperationException err)
        {
            return ResponseFactory.NotAcceptable($"Formato de dados inválido.: {err.Message}");;
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

    public async Task<ResponseModel> GetAllConsolesByUser(ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId();
            var res = await _userRepository.GetAllConsolesByUser(user_id, x => new UserConsole()
            {
                UserConsoleId = x.UserConsoleId,
                Console = x.Console,
                Condition = x.Condition,
                PurchaseDate = x.PurchaseDate,
                Notes = x.Notes,
                OwnershipStatus = x.OwnershipStatus
            });

            res.ForEach(x => x.MapObjectsTo(new GetAllConsolesByUserResponseModel()));
            return res.Ok();
        }
        catch (ArgumentNullException err)
        {
            return ResponseFactory.NotAcceptable($"Formato de dados inválido: {err.Message}");
        }
        catch (NullClaimException err)
        {
            return ResponseFactory.BadRequest(err.Message);
        }
    }
}
