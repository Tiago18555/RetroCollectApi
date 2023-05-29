using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using Game = RetroCollect.Models.Game;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public class ManageGameCollectionService : IManageGameCollectionService
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

        public async Task<ResponseModel> AddGame(AddGameRequestModel item, ClaimsPrincipal user)
        {
            //Specify format of DateTime entries yyyy-mm-dd

            var foundUser = userRepository.SingleOrDefault(u => u.UserId == item.User_id);
            if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

            if (!gameRepository.Any(g => g.GameId == item.Game_id))
            {
                //Função adicionar jogo na entity Game  =>
                var result = await searchGameService.RetrieveGameInfoAsync(item.Game_id);

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

                try
                {
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

            UserCollection userCollection = new()
            {
                ConsoleId = item.PlatformIsComputer == false ? item.Platform_id : 0,
                ComputerId = item.PlatformIsComputer == true ? item.Platform_id : 0,
                GameId = item.Game_id,
                UserId = item.User_id,
                Condition = Enum.Parse<Condition>(item.Condition.ToCapitalize(typeof(Condition))),
                OwnershipStatus = Enum.Parse<OwnershipStatus>(item.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                Notes = item.Notes == null ? null : item.Notes,
                PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
            };

            try
            {
                var res = userCollectionRepository.Add(userCollection);
                return res.MapObjectTo(new AddGameResponseModel()).Created();
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public ResponseModel DeleteGame(Guid id, ClaimsPrincipal user)
        {
            try
            {
                var foundItem = userCollectionRepository.GetById(id);
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
        }

        public async Task<ResponseModel> UpdateGame(UpdateGameRequestModel updateGameRequestModel, ClaimsPrincipal user)
        {
            
            try
            {
                var foundUser = userCollectionRepository.SingleOrDefault(x => x.UserId == updateGameRequestModel.User_id);

                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

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

                var res = this.userCollectionRepository.Update(foundUser.MapAndFill<UserCollection, UpdateGameRequestModel>(updateGameRequestModel));

                return res.Ok();
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
