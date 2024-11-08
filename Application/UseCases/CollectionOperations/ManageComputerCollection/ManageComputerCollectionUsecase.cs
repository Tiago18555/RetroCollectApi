using Domain;
using Application.UseCases.CollectionOperations.Shared;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;
using Domain.Repositories;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;

namespace Application.UseCases.CollectionOperations.ManageComputerCollection;

public class ManageComputerCollectionService : IManageComputerCollectionUsecase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserComputerRepository _userComputerRepository;
    private readonly IProducerService _producer;

    public ManageComputerCollectionService(
        IUserRepository userRepository, 
        IUserComputerRepository userComputerRepository, 
        IProducerService producer
    )
    {
        _userRepository = userRepository;
        _userComputerRepository = userComputerRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> AddComputer(AddItemRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var user = _userRepository.Any(u => u.UserId == user_id);
            if (!user) { return ResponseFactory.NotFound("User not found"); }
            
            var messageObject = new MessageModel{ Message = new {
                requestBody.ItemId,
                requestBody.Condition,
                requestBody.Notes,
                requestBody.PurchaseDate,
                requestBody.OwnershipStatus
            }, SourceType = "add-computer" };

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
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
    }

    public async Task<ResponseModel> DeleteComputer(Guid user_computer_id, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var foundItem = _userComputerRepository.SingleOrDefault(r => r.UserId == user_id && r.Id == user_computer_id);
            if (foundItem == null) { return ResponseFactory.NotFound(); }
            
            var messageObject = new MessageModel{ Message = foundItem, SourceType = "delete-computer" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "collection");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Deleted".Created(message = status);
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

    public async Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var foundComputer = _userComputerRepository.SingleOrDefault(x => x.Id == requestBody.Id);
            var foundUser = _userRepository.Any(x => x.UserId == user_id);

            if (!foundUser) 
                return ResponseFactory.NotFound("User not found");

            if (foundComputer == null) 
                return ResponseFactory.NotFound("Computer Not Found");
                
            var messageObject = new MessageModel { Message = new
                {
                    UserId = user_id,
                    requestBody.Id,
                    requestBody.PurchaseDate,
                    requestBody.Condition,
                    requestBody.OwnershipStatus,
                    requestBody.Notes
                },
                SourceType = "update-computer" 
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
            return ResponseFactory.NotAcceptable($"Formato de dados inválido.: {err.Message}");
        }
        catch (InvalidClassTypeException err)
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

    public async Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId(); 
            var res = await _userRepository.GetAllComputersByUser(user_id, x => new ComputerCollectionItem()
            {
                Id = x.Id,
                Computer = x.Computer,
                Condition = x.Condition,
                PurchaseDate = x.PurchaseDate,
                Notes = x.Notes,
                OwnershipStatus = x.OwnershipStatus
            });

            res.ForEach(x => x.MapObjectsTo(new GetAllComputersByUserResponseModel()));

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
