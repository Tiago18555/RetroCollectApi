using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchGame;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;
using System.Security.Claims;
using static RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems.ManageGameCollectionService;
using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public class ManageConsoleCollectionService : IManageConsoleCollectionService
    {
        private IUserRepository userRepository { get; set; }
        private IConsoleRepository consoleRepository { get; set; }
        private IUserConsoleRepository userConsoleRepository { get; set; }
        private ISearchConsoleService searchConsoleService { get; set; }
        public ManageConsoleCollectionService(
            IUserRepository userRepository,
            IConsoleRepository consoleRepository,
            IUserConsoleRepository userConsoleRepository,
            ISearchConsoleService searchConsoleService
        )
        {
            this.userRepository = userRepository;
            this.consoleRepository = consoleRepository;
            this.userConsoleRepository = userConsoleRepository;
            this.searchConsoleService = searchConsoleService;
        }


        public async Task<ResponseModel> AddConsole(AddItemRequestModel item, ClaimsPrincipal request)
        {
            //Specify format of DateTime entries yyyy-mm-dd

            var user = userRepository.SingleOrDefault(u => u.UserId == item.User_id);
            if (user == null) { return GenericResponses.NotFound("User not found"); }

            if (!consoleRepository.Any(g => g.ConsoleId == item.Item_id))
            {
                //Função adicionar console na entity Console=>
                var result = await searchConsoleService.RetrieveConsoleInfoAsync(item.Item_id);

                var consoleInfo = result.Single();

                Console console = new()
                {
                    ConsoleId = consoleInfo.ConsoleId,
                    Description = consoleInfo.Description,
                    ImageUrl = consoleInfo.ImageUrl,
                    Name = consoleInfo.Name,
                    IsPortable = consoleInfo.IsPortable
                };

                try
                {
                    var res = consoleRepository.Add(console);
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
                UserConsole userConsole = new()
                {
                    ConsoleId = item.Item_id,
                    UserId = item.User_id,
                    Condition = Enum.Parse<Condition>(item.Condition.ToCapitalize(typeof(Condition))),
                    OwnershipStatus = Enum.Parse<OwnershipStatus>(item.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                    Notes = item.Notes == null ? null : item.Notes,
                    PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
                };

                var res = userConsoleRepository.Add(userConsole);
                return res.MapObjectTo(new AddItemResponseModel()).Created();
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
        public ResponseModel DeleteConsole(Guid id, ClaimsPrincipal request)
        {
            try
            {
                var foundItem = userConsoleRepository.GetById(id);
                if (foundItem == null) { return GenericResponses.NotFound(); }

                if (userConsoleRepository.Delete(foundItem))
                {
                    return GenericResponses.Ok("Console deleted");
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

        public async Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel updateConsoleRequestModel, ClaimsPrincipal request)
        {
            try
            {
                var foundUser = userRepository.SingleOrDefault(x => x.UserId == updateConsoleRequestModel.User_id);
                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var foundConsole = userConsoleRepository.SingleOrDefault(x => x.UserConsoleId == updateConsoleRequestModel.UserConsoleId);
                if (foundConsole == null) { return GenericResponses.NotFound("Item Not Found"); }

                if (!consoleRepository.Any(g => g.ConsoleId == updateConsoleRequestModel.Item_id) && updateConsoleRequestModel.Item_id != 0)
                {
                    var result = await searchConsoleService.RetrieveConsoleInfoAsync(updateConsoleRequestModel.Item_id);

                    var consoleInfo = result.Single();

                    Console console = new()
                    {
                        ConsoleId = consoleInfo.ConsoleId,
                        Description = consoleInfo.Description,
                        ImageUrl = consoleInfo.ImageUrl,
                        Name= consoleInfo.Name,
                        IsPortable= consoleInfo.IsPortable                        
                    };
                    consoleRepository.Add(console);

                }

                var res = this.userConsoleRepository.Update(foundConsole.MapAndFill<UserConsole, UpdateConsoleRequestModel>(updateConsoleRequestModel));

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

        public async Task<ResponseModel> GetAllConsolesByUser(Guid userId, ClaimsPrincipal user)
        {
            if (user.IsTheRequestedOneId(userId)) { return GenericResponses.Unauthorized(); }

            try
            {
                var res = await userRepository.GetAllConsolesByUser(userId, x => x.MapObjectTo(new GetAllConsolesByUserResponseModel()));
                return res.Ok();
            }
            catch (ArgumentNullException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
        }
    }
}
