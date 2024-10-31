using System.Data;
using Domain;
using System.Text.Json;
using Domain.Broker;
using Domain.Repositories;
using CrossCutting;

namespace Application.UseCases.UserOperations.CreateUser;

public class CreateUserUsecase : ICreateUserUsecase
{
    private readonly IUserRepository _repository;
    private readonly IProducerService _producer;
    public CreateUserUsecase (
        IUserRepository repository, 
        IProducerService producer
    )
    {
        this._repository = repository;
        this._producer = producer;
    }

    public async Task<ResponseModel> CreateUser(CreateUserRequestModel request)
    {
        if (_repository.Any(x => x.Username == request.Username || x.Email == request.Email))        
            return ResponseFactory.Conflict();

        request.Username = Uri.EscapeDataString(request.Username);        

        try
        {
            var messageObject = new MessageModel{ Message = new 
            {
                request.Username,
                request.Email,
                request.Password,
                request.ConfirmPassword,
                request.FirstName,
                request.LastName
            }, SourceType = "create-user" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "user");

            StdOut.Error($"X: {JsonSerializer.Serialize(messageObject)}");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (DBConcurrencyException)
        {
            return ResponseFactory.NotAcceptable("Formato de dados inválido");
        }
        catch (InvalidOperationException)
        {
            return ResponseFactory.NotAcceptable("Formato de dados inválido.");
        }
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
    }
}
