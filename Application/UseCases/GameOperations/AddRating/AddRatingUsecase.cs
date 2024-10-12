using Domain;
using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Game = Domain.Entities.Game;
using Domain.Repositories;
using CrossCutting.Providers;
using CrossCutting;

namespace Application.UseCases.GameOperations.AddRating;

public class AddRatingUsecase : IAddRatingUsecase
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISearchGameUsecase _searchGameService;

    public AddRatingUsecase(IRatingRepository repository, IGameRepository gameRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider, ISearchGameUsecase searchGameService)
    {
        _ratingRepository = repository;
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _searchGameService = searchGameService;
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

            if (!_gameRepository.Any(g => g.GameId == game_id))
            {                    
                var result = await _searchGameService.RetrieveGameInfoAsync(game_id);

                var gameInfo = result.Single();

                Game game = new()
                {
                    GameId = gameInfo.GameId,
                    Genres = gameInfo.Genres,
                    Description = gameInfo.Description ?? "",
                    Summary = gameInfo.Summary ?? "",
                    ImageUrl = gameInfo.Cover ?? "",
                    Title = gameInfo.Title ?? "",
                    ReleaseYear = gameInfo.FirstReleaseDate
                };


                _gameRepository.Add(game);                    
            }

            var newRating = requestBody.MapObjectTo(new Rating());

            newRating.CreatedAt = _dateTimeProvider.UtcNow;
            newRating.UserId = user_id;

            return _ratingRepository.Add(newRating)
                .MapObjectsTo(new AddRatingResponseModel())
                .Created();
        }
        catch (NullClaimException msg)
        {
            return ResponseFactory.BadRequest(msg.ToString());
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        catch (DbUpdateException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
