using Domain;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;
using Domain.Repositories;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;

namespace Application.UseCases.UserCollectionOperations.ManageGameCollection;

public partial class ManageGameCollectionUsecase : IManageGameCollectionUsecase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserCollectionRepository _userCollectionRepository;
    private readonly IProducerService _producer;

    public ManageGameCollectionUsecase(
        IUserRepository userRepository, 
        IUserCollectionRepository userCollectionRepository, 
        IProducerService producer
    )
    {
        _userRepository = userRepository;
        _userCollectionRepository = userCollectionRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> AddGame(AddGameRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        var user_id = requestToken.GetUserId();

        var user = _userRepository.Any(u => u.UserId == user_id);
        if (!user) { return ResponseFactory.NotFound("User not found"); }

        try
        {
            var messageObject = new MessageModel{ 
                Message = new 
                {
                    UserId = user_id,
                    requestBody.GameId,
                    requestBody.PlatformId,
                    requestBody.PlatformIsComputer,
                    requestBody.PurchaseDate,
                    requestBody.Condition,
                    requestBody.OwnershipStatus,
                    requestBody.Notes
                }, 
                SourceType = "add-game" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (NullClaimException err)
        {
            return ResponseFactory.BadRequest(err.Message);
        }
        catch (InvalidEnumTypeException err)
        {
            return ResponseFactory.UnsupportedMediaType("Invalid type for Condition or OwnershipStatus: " + err.Message);
        }
        catch (InvalidEnumValueException err)
        {
            return ResponseFactory.BadRequest("Invalid value for Condition or OwnershipStatus: " + err.Message);
        }
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
        
    }

    public async Task<ResponseModel> DeleteGame(Guid user_collection_id, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var foundItem = _userCollectionRepository.SingleOrDefault(x => x.UserId == user_id && x.UserCollectionId == user_collection_id);
            if (foundItem == null) { return ResponseFactory.NotFound(); }

            var messageObject = new MessageModel{ Message = foundItem, SourceType = "delete-game" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Deleted".Ok(message = status);
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

    public async Task<ResponseModel> UpdateGame(UpdateGameRequestModel requestBody, ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId();

            var foundUser = _userRepository.SingleOrDefault(x => x.UserId == user_id);
            if (foundUser == null) { return ResponseFactory.NotFound("User not found"); }

            var foundGame = _userCollectionRepository.SingleOrDefault(x => x.UserCollectionId == requestBody.UserCollectionId);
            if (foundGame == null) { return ResponseFactory.NotFound($"Game Not Found"); }

            var messageObject = new MessageModel { Message = new 
                {
                    UserId = user_id,
                    requestBody.UserCollectionId,
                    requestBody.PurchaseDate,
                    requestBody.Condition,
                    requestBody.OwnershipStatus,
                    requestBody.Notes
                },
                SourceType = "update-game" 
            };

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
            return ResponseFactory.NotAcceptable($"Formato de dados inválido: {err.Message}.");
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

    public async Task<ResponseModel> GetAllGamesByUser(ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId();
            var res = await _userRepository.GetAllCollectionsByUser(user_id, x => new UserCollection()
            {
                UserCollectionId = x.UserCollectionId,
                Game = x.Game,
                Condition = x.Condition,
                PurchaseDate = x.PurchaseDate,
                Notes = x.Notes,
                OwnershipStatus = x.OwnershipStatus
            });

            res.ForEach(x => x.MapObjectsTo(new GetAllCollectionsByUserResponseModel()));

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
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
    }
}
