using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.CrossCutting.Validations;
using RetroCollectApi.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Game = RetroCollect.Models.Game;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public interface IAddItemService
    {
        public ResponseModel AddComputer(AddItemRequestModel item);
        public ResponseModel AddConsole(AddItemRequestModel item);
        public Task<ResponseModel> AddGame(AddGameRequestModel item);
    }

    public interface IDeleteItemService
    {
        public ResponseModel DeleteComputer(AddItemRequestModel item);
        public ResponseModel DeleteConsole(AddItemRequestModel item);
        public ResponseModel DeleteGame(AddItemRequestModel item);
    }

    public class DeleteItemService : IDeleteItemService
    {
        public ResponseModel DeleteComputer(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel DeleteConsole(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel DeleteGame(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }
    }

    public class AddItemService : IAddItemService
    {
        #region DEPENDENCY INJECTION

        private IUserRepository userRepository { get; set; }
        private IGameRepository gameRepository { get; set; }
        private IComputerRepository computerRepository { get; set; }
        private IConsoleRepository consoleRepository { get; set; }

        private IUserCollectionRepository userCollectionRepository { get; set; }
        private IUserComputerRepository userComputerRepository { get; set; }
        private IUserConsoleRepository userConsoleRepository { get; set; }

        private ISearchGameService searchGameService { get; set; }
        private ISearchComputerService searchComputerService { get; set; }
        private ISearchConsoleService searchConsoleService { get; set; }
        public AddItemService(
            IUserRepository userRepository,
            IGameRepository gameRepository, 
            IComputerRepository computerRepository,
            IConsoleRepository consoleRepository, 
            IUserCollectionRepository userCollectionRepository, 
            IUserComputerRepository userComputerRepository, 
            IUserConsoleRepository userConsoleRepository, 
            ISearchGameService searchGameService, 
            ISearchComputerService searchComputerService, 
            ISearchConsoleService searchConsoleService
        )
        {
            this.userRepository = userRepository;
            this.gameRepository = gameRepository;
            this.computerRepository = computerRepository;
            this.consoleRepository = consoleRepository;
            this.userCollectionRepository = userCollectionRepository;
            this.userComputerRepository = userComputerRepository;
            this.userConsoleRepository = userConsoleRepository;
            this.searchGameService = searchGameService;
            this.searchComputerService = searchComputerService;
            this.searchConsoleService = searchConsoleService;
        }

        #endregion

        public ResponseModel AddComputer(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public ResponseModel AddConsole(AddItemRequestModel item)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> AddGame(AddGameRequestModel item)
        {
            //Specify format of DateTime entries yyyy-mm-dd


            var user = userRepository.SingleOrDefault(u => u.UserId == item.User_id);
            if(user == null) { return GenericResponses.NotFound("User not found");  }

            if(!gameRepository.Any(g => g.GameId == item.Game_id))
            {
                //Função adicionar jogo na entity Game=>
                var result = await searchGameService.RetrieveGameInfoAsync(item.Game_id);

                var gameInfo = result.Single();

                Game game = new() {
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

            UserCollection userCollection = new()
            {
                ConsoleId = item.PlatformIsComputer == false ? item.Platform_id : 0,
                ComputerId = item.PlatformIsComputer == true ? item.Platform_id : 0,
                GameId = item.Game_id,
                UserId = item.User_id,
                Condition = (Condition) Enum.Parse(typeof(Condition), item.Condition),
                OwnershipStatus = (OwnershipStatus) Enum.Parse(typeof(OwnershipStatus), item.OwnershipStatus),
                Notes = item.Notes == null ? null : item.Notes,
                PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
            };

            try
            {
                var res = userCollectionRepository.Add(userCollection);
                return res.Created();
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
    }

    public class AddItemRequestModel
    {
        [Required]
        public int item_id { get; set; }

        [Required]
        public Guid user_id { get; set; }
    }

    public class AddGameRequestModel
    {
        [Required]
        public int Game_id { get; set; }

        [Required]
        public int Platform_id { get; set; }

        [Required]
        public bool PlatformIsComputer { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [CustomDate(ErrorMessage = "Invalid date format.")]
        [NotFutureDate(ErrorMessage = "Date cannot be in the future.")]
        public DateTime PurchaseDate { get; set; }

        [IsValidCondition]
        public string Condition { get; set; }

        [IsValidOwnershipStatus]
        public string OwnershipStatus { get; set; }

        public string Notes { get; set; }
    }

    public class AddItemResponseModel
    {

    }
}
