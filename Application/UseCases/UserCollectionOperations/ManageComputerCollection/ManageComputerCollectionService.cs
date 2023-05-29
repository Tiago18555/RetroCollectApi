using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public class ManageComputerCollectionService : IManageComputerCollectionService
    {
        private IUserRepository userRepository { get; set; }
        private IComputerRepository computerRepository { get; set; }
        private IUserComputerRepository userComputerRepository { get; set; }
        private ISearchComputerService searchComputerService { get; set; }
        public ManageComputerCollectionService(
            IUserRepository userRepository,
            IComputerRepository computerRepository,
            IUserComputerRepository userComputerRepository,
            ISearchComputerService searchComputerService
        )
        {
            this.userRepository = userRepository;
            this.computerRepository = computerRepository;
            this.userComputerRepository = userComputerRepository;
            this.searchComputerService = searchComputerService;
        }

        public async Task<ResponseModel> AddComputer(AddItemRequestModel item, ClaimsPrincipal request)
        {
            //Specify format of DateTime entries yyyy-mm-dd

            var user = userRepository.SingleOrDefault(u => u.UserId == item.User_id);
            if (user == null) { return GenericResponses.NotFound("User not found"); }

            if (!computerRepository.Any(g => g.ComputerId == item.Item_id))
            {
                //Função adicionar computer na entity Computer=>
                var result = await searchComputerService.RetrieveComputerInfoAsync(item.Item_id);

                var computerInfo = result.Single();

                Computer computer = new()
                {
                    ComputerId = computerInfo.ComputerId,
                    Description = computerInfo.Description,
                    ImageUrl = computerInfo.ImageUrl,
                    Name = computerInfo.Name,
                    IsArcade = computerInfo.IsArcade
                };

                try
                {
                    var res = computerRepository.Add(computer);
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

            UserComputer userComputer = new()
            {
                ComputerId = item.Item_id,
                UserId = item.User_id,
                Condition = Enum.Parse<Condition>(item.Condition.ToCapitalize(typeof(Condition))),
                OwnershipStatus = Enum.Parse<OwnershipStatus>(item.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                Notes = item.Notes == null ? null : item.Notes,
                PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
            };

            try
            {
                var res = userComputerRepository.Add(userComputer);
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

        public ResponseModel DeleteComputer(Guid id, ClaimsPrincipal request)
        {
            try
            {
                var foundItem = userComputerRepository.GetById(id);
                if (foundItem == null) { return GenericResponses.NotFound(); }

                if (userComputerRepository.Delete(foundItem))
                {
                    return GenericResponses.Ok("Computer deleted");
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

        public Task<ResponseModel> UpdateGame(UpdateComputerRequestModel item, ClaimsPrincipal request)
        {
            throw new NotImplementedException();
        }
    }
}
