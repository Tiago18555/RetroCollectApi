using CrossCutting;
using CrossCutting.Providers;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.ManageRating;

public class ManageRatingUsecase : IManageRatingUsecase
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ManageRatingUsecase(IRatingRepository repository, IGameRepository gameRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
    {
        _ratingRepository = repository;
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public ResponseModel EditRating(EditRatingRequestModel requestBody, ClaimsPrincipal requestToken)
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


            foundRating.RatingValue = requestBody.RatingValue == 0 ? foundRating.RatingValue : requestBody.RatingValue;
            foundRating.Review = String.IsNullOrEmpty(requestBody.Review) ? foundRating.Review : requestBody.Review;
            foundRating.UpdatedAt = _dateTimeProvider.UtcNow;

            return
                _ratingRepository.Update(foundRating)
                .MapObjectsTo(new EditRatingResponseModel())
                .Ok();
        }
        catch (DBConcurrencyException)
        {
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
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

    public ResponseModel RemoveRating(Guid ratingId, ClaimsPrincipal requestToken)
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

            var success = _ratingRepository.Delete(foundRating);

            if (success)                
                return "Rating Deleted".Ok();

            return ResponseFactory.ServiceUnavailable($"Unknown error at {System.Environment.CurrentDirectory} on RemoveRating");
        }
        catch (ArgumentNullException e)
        {
            return ResponseFactory.ServiceUnavailable(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return ResponseFactory.ServiceUnavailable(e.Message);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
public class RatingResponseModel
{

}
