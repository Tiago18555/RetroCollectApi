using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;
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

        public async Task<ResponseModel> AddGame(AddGameRequestModel item)
        {
            //Specify format of DateTime entries yyyy-mm-dd

            var user = userRepository.SingleOrDefault(u => u.UserId == item.User_id);
            if (user == null) { return GenericResponses.NotFound("User not found"); }

            if (!gameRepository.Any(g => g.GameId == item.Game_id))
            {
                //Função adicionar jogo na entity Game=>
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
                    var res = gameRepository.Add(game);
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

            //item.OwnershipStatus.

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

        public Task<ResponseModel> DeleteGame()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> UpdateGame(UpdateGameRequestModel item)
        {
            throw new NotImplementedException();
        }
    }

    public class AddGameResponseModel
    {

        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int ComputerId { get; set; }
        public int ConsoleId { get; set; }
    }
}
