using CrossCutting;
using Domain;
using Domain.Broker;
using Domain.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace Application.UseCases.UserWishlistOperations;

public class WishlistUsecase : IWishlistUsecase
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IProducerService _producer;

    public WishlistUsecase(IWishlistRepository wishlistRepository, IUserRepository userRepository, IGameRepository gameRepository, IProducerService producer)
    {
        _wishlistRepository = wishlistRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _producer = producer;
    }

    public async Task<ResponseModel> Add(AddToUserWishlistRequestModel requestBody, ClaimsPrincipal requestToken)
    {

        var user_id = requestToken.GetUserId();
        var item_id = requestBody.Id;

        var user = _userRepository.Any(u => u.UserId == user_id);

        if (!user) { return ResponseFactory.NotFound("User not found"); }

        var game = _gameRepository.Any(g => g.GameId == item_id);

        if (!game) { return ResponseFactory.NotFound("Game not found"); }

        try
        {
            var messageObject = new MessageModel{ Message = new {
                UserId = user_id,
                GameId = item_id
            }, SourceType = "add-wishlist" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "wishlist");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (Exception err)
        {
            return ResponseFactory.ServiceUnavailable(err.Message);
        }

    }

    public async Task<ResponseModel> GetAllByGame(int game_id, int limit, int page)
    {
        if (! _gameRepository.Any(u => u.GameId == game_id))
            return ResponseFactory.NotFound("Game not found");

        List<object> result;

        if(limit == 0)            
            result = await _wishlistRepository.GetWishlistsByGame(game_id, x => new {
                id = x.WishlistId,
                user = x.User.Username ?? ""
            } as object);
        else
            result = await _wishlistRepository.GetWishlistsByGame(game_id, x => new {
                id = x.WishlistId,
                user = x.User.Username ?? ""
            } as object, page, limit);

        return result.Ok();
    }

    public async Task<ResponseModel> GetAllByUser(ClaimsPrincipal RequestToken, int limit = 0, int page = 0)
    {
        var user_id = RequestToken.GetUserId();

        if (! _userRepository.Any(u => u.UserId == user_id))
            return ResponseFactory.NotFound("User not found");

        List<object> result;

        if(limit == 0)            
            result = await _wishlistRepository.GetWishlistsByUser(user_id, x => new {
                id = x.WishlistId,
                game = x.Game.Title ?? ""
            } as object);
        else
            result = await _wishlistRepository.GetWishlistsByUser(user_id, x => new {
                id = x.WishlistId,
                game = x.Game.Title ?? ""
            } as object, page, limit);

        return result.Ok();
    }

    public async Task<ResponseModel> Remove(int game_id, ClaimsPrincipal RequestToken)
    {
        var user_id = RequestToken.GetUserId();

        var user = _userRepository.Any(u => u.UserId == user_id);

        if (!user) { return ResponseFactory.NotFound("User not found"); }

        var game = _gameRepository.Any(g => g.GameId == game_id);

        if (!game) { return ResponseFactory.NotFound("Game not found"); }

        try
        {
            var messageObject = new MessageModel{ Message = new {
                UserId = user_id,
                GameId = game_id
            }, SourceType = "remove-wishlist" };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "wishlist");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Removed successfully".Ok(message = status);
        }
        catch (Exception)
        {
            return ResponseFactory.NotFound();
        }
    }
}
 
