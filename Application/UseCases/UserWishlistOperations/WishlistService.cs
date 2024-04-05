﻿using CrossCutting;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using System.Security.Claims;

namespace Application.UseCases.UserWishlistOperations
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
            var item_id = RequestBody.Id;

            var user = _userRepository.Any(u => u.UserId == user_id);

            if (!user) { return GenericResponses.NotFound("User not found"); }

            var game = _gameRepository.Any(g => g.GameId == item_id);

            if (!game) { return GenericResponses.NotFound("Game not found"); }

            var wishlist = new Wishlist { UserId = user_id, GameId = item_id };

            return _repository
                .Add(wishlist)
                .MapObjectTo(
                    new AddToUserWishlistResponseModel()
                 ).Ok();
        }

        public async Task<ResponseModel> GetAllByGame(int gameId, int limit, int page)
        {
            if (_gameRepository.Any(u => u.GameId == gameId))
                return GenericResponses.NotFound("Game not found");

            List<WishlistResponseModel> result;

            if (limit == 0)
                result = await _repository.GetWishlistsByGame(gameId, x => x.MapObjectTo(new WishlistResponseModel()));
            else
                result = await _repository.GetWishlistsByGame(gameId, x => x.MapObjectTo(new WishlistResponseModel()), page, limit);

            return result.Ok();
        }

        public async Task<ResponseModel> GetAllByUser(ClaimsPrincipal RequestToken, int limit = 0, int page = 0)
        {
            var user_id = RequestToken.GetUserId();

            if (_userRepository.Any(u => u.UserId == user_id))
                return GenericResponses.NotFound("User not found");

            List<WishlistResponseModel> result;

            if(limit == 0)            
                result = await _repository.GetWishlistsByUser(user_id, x => x.MapObjectTo(new WishlistResponseModel()));
            else
                result = await _repository.GetWishlistsByUser(user_id, x => x.MapObjectTo(new WishlistResponseModel()), page, limit);

            return result.Ok();
        }

        public ResponseModel Remove(int game_id, ClaimsPrincipal RequestToken)
        {
            var user_id = RequestToken.GetUserId();

            var user = _userRepository.Any(u => u.UserId == user_id);

            if (!user) { return GenericResponses.NotFound("User not found"); }

            var game = _gameRepository.Any(g => g.GameId == game_id);

            if (!game) { return GenericResponses.NotFound("Game not found"); }

            var result = _repository.Delete(new Wishlist { UserId = user_id, GameId = game_id });

            if (result) return "Successfully Deleted".Ok();
            else return GenericResponses.NotFound("Operation not successfully completed");
        }
    }
     
}
