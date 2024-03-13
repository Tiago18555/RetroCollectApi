using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Providers;
using RetroCollectApi.Repositories.Interfaces;

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

        public ResponseModel AddRating(AddRatingRequestModel request)
        {
            if (_userRepository.Any(u => u.UserId == request.UserId))
                return GenericResponses.NotFound("User not found");

            if (_gameRepository.Any(g => g.GameId == request.GameId))
                return GenericResponses.NotFound("Game not found");

            if (_repository.Any(r => r.UserId == request.UserId && r.GameId == request.GameId))
                return GenericResponses.BadRequest("User cannot have 2 ratings on the same game");

            try
            {
                var newRating = request.MapObjectTo(new Rating());

                newRating.CreatedAt = _dateTimeProvider.UtcNow;
              
                return _repository.Add(newRating)
                    .MapObjectTo(new AddRatingResponseModel())
                    .Created();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
