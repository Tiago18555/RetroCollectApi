using Domain;
using Domain.Exceptions;

using System.Security.Claims;
using Domain.Repositories;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;

namespace Application.UseCases.RatingOperations.AddRating;

public class AddRatingUsecase : IAddRatingUsecase
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProducerService _producer;

    public AddRatingUsecase (
        IRatingRepository ratingRepository, 
        IUserRepository userRepository, 
        IProducerService producer
    )
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();
            var game_id = requestBody.GameId;

            if (!_userRepository.Any(u => u.UserId == user_id))
                return ResponseFactory.NotFound($"User {user_id} not found");


            if (_ratingRepository.Any(r => r.UserId == user_id && r.GameId == game_id))
                return ResponseFactory.BadRequest("User cannot have 2 ratings on the same game");

            var messageObject = new MessageModel{ Message = new 
            {
                UserId = user_id,
                GameId = game_id,
                requestBody.RatingValue,
                requestBody.Review
            }, 
            SourceType = "add-rating" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "rating");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (NullClaimException msg)
        {
            return ResponseFactory.BadRequest(msg.ToString());
        }
        catch (InvalidOperationException err)
        {
            return ResponseFactory.ServiceUnavailable(err.ToString());
        }
        catch (ArgumentNullException err)
        {
            return ResponseFactory.ServiceUnavailable(err.ToString());
        }
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.ToString());
        }
    }
}
