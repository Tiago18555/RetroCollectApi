using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;

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

        public async Task<ResponseModel> AddComputer(AddItemRequestModel item)
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
                Condition = (Condition)Enum.Parse(typeof(Condition), item.Condition),
                OwnershipStatus = (OwnershipStatus)Enum.Parse(typeof(OwnershipStatus), item.OwnershipStatus),
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

        public Task<ResponseModel> DeleteComputer()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> UpdateGame(UpdateComputerRequestModel item)
        {
            throw new NotImplementedException();
        }
    }
}
