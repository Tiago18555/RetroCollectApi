using CrossCutting;
using Application.UseCases.IgdbIntegrationOperations.SearchGame;
using Application.UseCases.UserCollectionOperations.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Game = Domain.Entities.Game;

namespace Application.UseCases.UserCollectionOperations.AddItems
{
    public partial class ManageGameCollectionService : IManageGameCollectionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserCollectionRepository _userCollectionRepository;
        private readonly ISearchGameService _searchGameService;
        public ManageGameCollectionService(
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IUserCollectionRepository userCollectionRepository,
            ISearchGameService searchGameService
        )
        {
            this._userRepository = userRepository;
            this._gameRepository = gameRepository;
            this._userCollectionRepository = userCollectionRepository;
            this._searchGameService = searchGameService;
        }

        public async Task<ResponseModel> AddGame(AddGameRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            var user_id = requestToken.GetUserId();

            var user = _userRepository.Any(u => u.UserId == user_id);
            if (!user) { return GenericResponses.NotFound("User not found"); }

            if (!_gameRepository.Any(g => g.GameId == requestBody.Game_id))
            {
                try
                {
                    var result = await _searchGameService.RetrieveGameInfoAsync(requestBody.Game_id);

                    var gameInfo = result.Single();

                    Game game = new()
                    {
                        GameId = gameInfo.GameId,
                        Genres = gameInfo.Genres,
                        Description = gameInfo.Description,
                        Summary = gameInfo.Summary,
                        ImageUrl = gameInfo.Cover,
                        Title = gameInfo.Title,
                        ReleaseYear = gameInfo.FirstReleaseDate
                    };


                    _gameRepository.Add(game);
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
            }

            try
            {
                UserCollection userCollection = new()
                {
                    ConsoleId = requestBody.PlatformIsComputer == false ? requestBody.Platform_id : 0,
                    ComputerId = requestBody.PlatformIsComputer == true ? requestBody.Platform_id : 0,
                    GameId = requestBody.Game_id,
                    UserId = requestBody.User_id,
                    Condition = Enum.Parse<Condition>(requestBody.Condition.ToCapitalize(typeof(Condition))),
                    OwnershipStatus = Enum.Parse<OwnershipStatus>(requestBody.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                    Notes = requestBody.Notes == null ? null : requestBody.Notes,
                    PurchaseDate = requestBody.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : requestBody.PurchaseDate
                };

                var res = _userCollectionRepository.Add(userCollection);
                return res.MapObjectsTo(new AddGameResponseModel()).Created();
            }
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (InvalidEnumTypeException msg)
            {
                return GenericResponses.UnsupportedMediaType("Invalid type for Condition or OwnershipStatus: " + msg);
            }
            catch (InvalidEnumValueException msg)
            {
                return GenericResponses.BadRequest("Invalid value for Condition or OwnershipStatus: " + msg);
            }
            
        }

        public ResponseModel DeleteGame(Guid user_collection_id, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();

                var foundItem = _userCollectionRepository.SingleOrDefault(x => x.UserId == user_id && x.UserCollectionId == user_collection_id);
                if (foundItem == null) { return GenericResponses.NotFound(); }

                if (_userCollectionRepository.Delete(foundItem))
                {
                    return GenericResponses.Ok("Game deleted");
                }
                else
                {
                    return GenericResponses.Ok("Not deleted");
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
        }

        public async Task<ResponseModel> UpdateGame(UpdateGameRequestModel updateGameRequestModel, ClaimsPrincipal user)
        {

            try
            {
                if (!user.IsTheRequestedOneId(updateGameRequestModel.User_id)) return GenericResponses.Forbidden();
                var foundUser = _userRepository.SingleOrDefault(x => x.UserId == updateGameRequestModel.User_id);
                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var foundGame = _userCollectionRepository.SingleOrDefault(x => x.UserCollectionId == updateGameRequestModel.UserCollection_id);
                if (foundGame == null) { return GenericResponses.NotFound("Item Not Found"); }

                if (!_gameRepository.Any(g => g.GameId == updateGameRequestModel.Game_id) && updateGameRequestModel.Game_id != 0)
                {
                    var result = await _searchGameService.RetrieveGameInfoAsync(updateGameRequestModel.Game_id);

                    var gameInfo = result.Single();

                    Game game = new()
                    {
                        GameId = gameInfo.GameId,
                        Genres = gameInfo.Genres,
                        Description = gameInfo.Description,
                        Summary = gameInfo.Summary,
                        ImageUrl = gameInfo.Cover,
                        Title = gameInfo.Title,
                        ReleaseYear = gameInfo.FirstReleaseDate
                    };

                    _gameRepository.Add(game);

                }

                var res = this._userCollectionRepository.Update(foundGame.MapAndFill<UserCollection, UpdateGameRequestModel>(updateGameRequestModel));

                return res.MapObjectsTo(new UpdateGameResponseModel()).Ok();
            }
            catch (ArgumentNullException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DBConcurrencyException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (DbUpdateException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (InvalidOperationException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido.");
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

        public async Task<ResponseModel> GetAllGamesByUser(ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();
                var res = await _userRepository.GetAllCollectionsByUser(user_id, x => new UserCollection()
                {
                    UserCollectionId = x.UserCollectionId,
                    Game = x.Game,
                    Condition = x.Condition,
                    PurchaseDate = x.PurchaseDate,
                    Notes = x.Notes,
                    OwnershipStatus = x.OwnershipStatus
                });

                res.ForEach(x => x.MapObjectsTo(new GetAllCollectionsByUserResponseModel()));

                return res.Ok();
            }
            catch (ArgumentNullException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
        }

        /*
                         var user_id = requestToken.GetUserId();
                var res = await userRepository.GetAllConsolesByUser(user_id, x => new UserConsole()
                {
                    UserConsoleId = x.UserConsoleId,
                    Console = x.Console,
                    Condition = x.Condition,
                    PurchaseDate = x.PurchaseDate,
                    Notes = x.Notes,
                    OwnershipStatus = x.OwnershipStatus
                });

                res.ForEach(x => x.MapObjectsTo(new GetAllConsolesByUserResponseModel()));
                return res.Ok();
         */
    }
}
