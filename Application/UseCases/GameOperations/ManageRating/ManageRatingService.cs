using CrossCutting;
using CrossCutting.Providers;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.ManageRating
{
    public class ManageRatingService : IManageRatingService
    {
        private IRatingRepository _repository;
        private IGameRepository _gameRepository;
        private IUserRepository _userRepository;
        private IDateTimeProvider _dateTimeProvider;

        public ManageRatingService(IRatingRepository repository, IGameRepository gameRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
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

                var foundRating = _repository.SingleOrDefault(x => x.RatingId == rating_id);

                if (foundRating == null)
                    return GenericResponses.NotFound($"Rating {rating_id} not found");

                if (foundRating.UserId != user_id)
                    return GenericResponses.Forbidden($"This rating {rating_id} is invalid");


                foundRating.RatingValue = requestBody.RatingValue == 0 ? foundRating.RatingValue : requestBody.RatingValue;
                foundRating.Review = String.IsNullOrEmpty(requestBody.Review) ? foundRating.Review : requestBody.Review;
                foundRating.UpdatedAt = _dateTimeProvider.UtcNow;

                return
                    _repository.Update(foundRating)
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
                return GenericResponses.BadRequest(msg.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResponseModel RemoveRating(Guid ratingId, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();
                var rating_id = ratingId;

                var foundRating = _repository.SingleOrDefault(x => x.RatingId == rating_id);
                if (foundRating == null)
                    return GenericResponses.NotFound("Rating not found");

                if (foundRating.UserId != user_id)
                    return GenericResponses.Forbidden("This rating is invalid");

                var success = _repository.Delete(foundRating);

                if (success)                
                    return "Rating Deleted".Ok();

                return GenericResponses.ServiceUnavailable($"Unknown error at {System.Environment.CurrentDirectory} on RemoveRating");
            }
            catch (ArgumentNullException e)
            {
                return GenericResponses.ServiceUnavailable(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return GenericResponses.ServiceUnavailable(e.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
