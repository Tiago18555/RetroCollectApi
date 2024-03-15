using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories;
using RetroCollectApi.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using Game = RetroCollect.Models.Game;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public partial class ManageGameCollectionService : IManageGameCollectionService
    {
        private IUserRepository userRepository { get; set; }
        private IGameRepository gameRepository { get; set; }
        private IUserCollectionRepository userCollectionRepository { get; set; }
        private ISearchGameService searchGameService { get; set; }
        public ManageGameCollectionService(
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IUserCollectionRepository userCollectionRepository,
            ISearchGameService searchGameService
        )
        {
            this.userRepository = userRepository;
            this.gameRepository = gameRepository;
            this.userCollectionRepository = userCollectionRepository;
            this.searchGameService = searchGameService;
        }

        public async Task<ResponseModel> AddGame(AddGameRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            var user_id = requestToken.GetUserId();

            var user = userRepository.Any(u => u.UserId == user_id);
            if (!user) { return GenericResponses.NotFound("User not found"); }

            if (!gameRepository.Any(g => g.GameId == requestBody.Game_id))
            {
                try
                {
                    var result = await searchGameService.RetrieveGameInfoAsync(requestBody.Game_id);

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


                    gameRepository.Add(game);
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

                var res = userCollectionRepository.Add(userCollection);
                return res.MapObjectTo(new AddGameResponseModel()).Created();
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

                var foundItem = userCollectionRepository.SingleOrDefault(x => x.UserId == user_id && x.UserCollectionId == user_collection_id);
                if (foundItem == null) { return GenericResponses.NotFound(); }

                if (userCollectionRepository.Delete(foundItem))
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
                var foundUser = userRepository.SingleOrDefault(x => x.UserId == updateGameRequestModel.User_id);
                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var foundGame = userCollectionRepository.SingleOrDefault(x => x.UserCollectionId == updateGameRequestModel.UserCollection_id);
                if (foundGame == null) { return GenericResponses.NotFound("Item Not Found"); }

                if (!gameRepository.Any(g => g.GameId == updateGameRequestModel.Game_id) && updateGameRequestModel.Game_id != 0)
                {
                    var result = await searchGameService.RetrieveGameInfoAsync(updateGameRequestModel.Game_id);

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

                    gameRepository.Add(game);

                }

                var res = this.userCollectionRepository.Update(foundGame.MapAndFill<UserCollection, UpdateGameRequestModel>(updateGameRequestModel));

                return res.MapObjectTo(new UpdateGameResponseModel()).Ok();
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
                var res = await userRepository.GetAllCollectionsByUser(user_id, x => x.MapObjectTo(new GetAllCollectionsByUserResponseModel()));
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
    }
}
