using CrossCutting;
using Domain;
using Domain.Broker;
using Domain.Exceptions;
using Domain.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace Application.UseCases.RatingOperations.ManageRating;

public class ManageRatingUsecase : IManageRatingUsecase
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProducerService _producer;

    public ManageRatingUsecase (
        IRatingRepository ratingRepository, 
        IGameRepository gameRepository, 
        IUserRepository userRepository, 
        IProducerService producer
    )
    {
        _ratingRepository = ratingRepository;
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> EditRating(EditRatingRequestModel requestBody, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();
            var rating_id = requestBody.RatingId;

            var foundRating = _ratingRepository.SingleOrDefault(x => x.RatingId == rating_id);

            if (foundRating == null)
                return ResponseFactory.NotFound($"Rating {rating_id} not found");

            if (foundRating.UserId != user_id)
                return ResponseFactory.Forbidden($"This rating {rating_id} is invalid");

            var messageObject = new MessageModel{ Message = new {
                requestBody.Review,
                requestBody.RatingValue,
                requestBody.RatingId
            }, SourceType = "edit-rating" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "rating");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Updated successfully".Ok(message = status);
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

    public async Task<ResponseModel> GetAllRatingsByGame(int gameId, int pageSize, int pageNumber)
    {
        if (_gameRepository.Any(u => u.GameId == gameId))
            return ResponseFactory.NotFound("Game not found");

        List<RatingResponseModel> result;

        if (pageSize == 0)
            result = await _ratingRepository.GetRatingsByGame(gameId, x => x.MapObjectTo(new RatingResponseModel()));
        else
            result = await _ratingRepository.GetRatingsByGame(gameId, x => x.MapObjectTo(new RatingResponseModel()), pageSize, pageNumber);

        return result.Ok();
    }

    public async Task<ResponseModel> GetAllRatingsByUser(ClaimsPrincipal requestToken, int pageSize, int pageNumber)
    {
        var user_id = requestToken.GetUserId();

        if (_userRepository.Any(u => u.UserId == user_id))
            return ResponseFactory.NotFound("User not found");

        List<RatingResponseModel> result;

        if (pageSize == 0)
            result = await _ratingRepository.GetRatingsByUser(user_id, x => x.MapObjectTo(new RatingResponseModel()));
        else
            result = await _ratingRepository.GetRatingsByUser(user_id, x => x.MapObjectTo(new RatingResponseModel()), pageSize, pageNumber);

        return result.Ok();
    }

    public async Task<ResponseModel> RemoveRating(Guid ratingId, ClaimsPrincipal requestToken)
    {
        try
        {
            var user_id = requestToken.GetUserId();
            var rating_id = ratingId;

            var foundRating = _ratingRepository.SingleOrDefault(x => x.RatingId == rating_id);
            if (foundRating == null)
                return ResponseFactory.NotFound("Rating not found");

            if (foundRating.UserId != user_id)
                return ResponseFactory.Forbidden("This rating is invalid");

            var messageObject = new MessageModel{ Message = foundRating, SourceType = "edit-rating" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "rating");

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
        catch (InvalidOperationException err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }
    }
}
