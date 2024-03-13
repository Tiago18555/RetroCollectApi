using Org.BouncyCastle.Asn1.Ocsp;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Providers;
using RetroCollectApi.Repositories.Interfaces;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class AddRatingService : IAddRatingService
    {
        private IRatingRepository _repository;
        private IGameRepository _gameRepository;
        private IUserRepository _userRepository;
        private IDateTimeProvider _dateTimeProvider;

        public AddRatingService(IRatingRepository repository, IGameRepository gameRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public ResponseModel AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();
                var game_id = requestBody.GameId;

                if (_userRepository.Any(u => u.UserId == user_id))
                    return GenericResponses.NotFound("User not found");

                if (_gameRepository.Any(g => g.GameId == game_id))
                    return GenericResponses.NotFound("Game not found");

                if (_repository.Any(r => r.UserId == user_id && r.GameId == game_id))
                    return GenericResponses.BadRequest("User cannot have 2 ratings on the same game");

                var newRating = requestBody.MapObjectTo(new Rating());

                newRating.CreatedAt = _dateTimeProvider.UtcNow;
              
                return _repository.Add(newRating)
                    .MapObjectTo(new AddRatingResponseModel())
                    .Created();
            }
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
