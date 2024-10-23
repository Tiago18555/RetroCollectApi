using Domain;
using Application.UseCases.UserCollectionOperations.Shared;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Domain.Repositories;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection;

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
        var user_id = requestToken.GetUserId();

        var user = _userRepository.Any(u => u.UserId == user_id);
        if (!user) { return ResponseFactory.NotFound("User not found"); }

        try
        {
            var messageObject = new MessageModel{ Message = requestBody, SourceType = "add-computer" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (DBConcurrencyException)
        {
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        catch (InvalidEnumTypeException msg)
        {
            return ResponseFactory.UnsupportedMediaType("Invalid type for Condition or OwnershipStatus: " + msg);
        }
        catch (InvalidEnumValueException msg)
        {
            return ResponseFactory.BadRequest("Invalid value for Condition or OwnershipStatus: " + msg);
        }
    }

    public async Task<ResponseModel> DeleteComputer(Guid user_computer_id, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();

            var foundItem = _userComputerRepository.SingleOrDefault(r => r.UserId == user_id && r.UserComputerId == user_computer_id);
            if (foundItem == null) { return ResponseFactory.NotFound(); }
            
            var messageObject = new MessageModel{ Message = foundItem, SourceType = "delete-computer" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Deleted".Created(message = status);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (NullClaimException msg)
        {
            return ResponseFactory.BadRequest(msg.ToString());
        }
    }

    public async Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel requestBody, ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId();

            var foundComputer = _userComputerRepository.SingleOrDefault(x => x.UserComputerId == requestBody.UserComputerId);
            var foundUser = _userRepository.Any(x => x.UserId == user_id);

            if (!foundUser) 
                return ResponseFactory.NotFound("User not found");

            if (foundComputer == null) 
                return ResponseFactory.NotFound("Computer Not Found");
                
            var messageObject = new MessageModel { Message = new
                {
                    UserId = user_id,
                    requestBody.UserComputerId,
                    requestBody.PurchaseDate,
                    requestBody.Condition,
                    requestBody.OwnershipStatus,
                    requestBody.Notes
                },
                SourceType = "update-computer" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Updated successfully".Ok(message = status);

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
        catch (InvalidClassTypeException msg)
        {
            //throw;
            return ResponseFactory.ServiceUnavailable(msg.ToString());
        }
        catch (NullClaimException msg)
        {
            return ResponseFactory.BadRequest(msg.ToString());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal requestToken)
    {

        try
        {
            var user_id = requestToken.GetUserId(); 
            var res = await _userRepository.GetAllComputersByUser(user_id, x => new UserComputer()
            {
                UserComputerId = x.UserComputerId,
                Computer = x.Computer,
                Condition = x.Condition,
                PurchaseDate = x.PurchaseDate,
                Notes = x.Notes,
                OwnershipStatus = x.OwnershipStatus
            });

            res.ForEach(x => x.MapObjectsTo(new GetAllComputersByUserResponseModel()));

            return res.Ok();
        }
        catch (ArgumentNullException)
        {
            throw;
            //return GenericResponses.NotAcceptable("Formato de dados inválido");
        }
        catch (NullClaimException msg)
        {
            return ResponseFactory.BadRequest(msg.ToString());
        }
    }
}
