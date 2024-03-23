using CrossCutting;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using System.Security.Claims;

namespace Application.UseCases.UserWishlistOperations.GameOperations
{
    public class WishlistService : IWishlistService
    {
        private IWishlistRepository _repository;
        private IUserRepository _userRepository;
        private IGameRepository _gameRepository;

        public WishlistService(IWishlistRepository repository, IUserRepository userRepository, IGameRepository gameRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
        }

        public ResponseModel Add(AddToUserWishlistRequestModel RequestBody, ClaimsPrincipal RequestToken)
        {
            
            var user_id = RequestToken.GetUserId();
            var game_id = RequestBody.GameId;

            var user = _userRepository.Any(u => u.UserId == user_id);

            if (!user) { return GenericResponses.NotFound("User not found"); }

            var game = _gameRepository.Any(g => g.GameId == game_id);

            if (!game) { return GenericResponses.NotFound("Game not found"); }

            var wishlist = new Wishlist { UserId = user_id, GameId = game_id };

            return _repository
                .Add(wishlist)
                .MapObjectTo(
                    new AddToUserWishlistResponseModel()
                 ).Ok();
        }

        public ResponseModel Remove(int game_id, ClaimsPrincipal RequestToken)
        {
            var user_id = RequestToken.GetUserId();

            var user = _userRepository.Any(u => u.UserId == user_id);

            if (!user) { return GenericResponses.NotFound("User not found"); }

            var game = _gameRepository.Any(g => g.GameId == game_id);

            if (!game) { return GenericResponses.NotFound("Game not found"); }

            var result = _repository.Delete(new Wishlist { UserId = user_id, GameId = game_id});

            if (result) return "Successfully Deleted".Ok();
            else return GenericResponses.NotFound("Operation not successfully completed");
        }
    }
}
