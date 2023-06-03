using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
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
            if (!request.IsTheRequestedOneId(item.User_id)) return GenericResponses.Forbidden();


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


            try
            {
                UserComputer userComputer = new()
                {
                    ComputerId = item.Item_id,
                    UserId = item.User_id,
                    Condition = Enum.Parse<Condition>(item.Condition.ToCapitalize(typeof(Condition))),
                    OwnershipStatus = Enum.Parse<OwnershipStatus>(item.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                    Notes = item.Notes == null ? null : item.Notes,
                    PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
                };

                var res = userComputerRepository.Add(userComputer);
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

        public ResponseModel DeleteComputer(Guid id, ClaimsPrincipal request)
        {
            try
            {
                var foundItem = userComputerRepository.GetById(id);
                if (foundItem == null) return GenericResponses.NotFound();
                if (!request.IsTheRequestedOneId(foundItem.UserId)) return GenericResponses.Forbidden();

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

        public async Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel updateComputerRequestModel, ClaimsPrincipal request)
        {
            if (!request.IsTheRequestedOneId(updateComputerRequestModel.User_id)) return GenericResponses.Forbidden();

            try
            {
                var foundUser = userRepository.SingleOrDefault(x => x.UserId == updateComputerRequestModel.User_id);
                if (foundUser == null) { return GenericResponses.NotFound("User not found"); }

                var foundComputer = userComputerRepository.SingleOrDefault(x => x.UserComputerId == updateComputerRequestModel.UserComputerId);
                if (foundComputer == null) { return GenericResponses.NotFound("Item Not Found"); }

                if (!computerRepository.Any(g => g.ComputerId == updateComputerRequestModel.Item_id) && updateComputerRequestModel.Item_id != 0)
                {
                    var result = await searchComputerService.RetrieveComputerInfoAsync(updateComputerRequestModel.Item_id);

                    var computerInfo = result.Single();

                    Computer computer = new()
                    {
                        ComputerId = computerInfo.ComputerId,
                        Description = computerInfo.Description,
                        ImageUrl = computerInfo.ImageUrl,
                        Name = computerInfo.Name,
                        IsArcade = computerInfo.IsArcade
                    };
                    computerRepository.Add(computer);

                }

                var res = this.userComputerRepository.Update(foundComputer.MapAndFill<UserComputer, UpdateComputerRequestModel>(updateComputerRequestModel));

                return res.MapObjectTo(new UpdateComputerResponseModel()).Ok();
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
            catch (InvalidClassTypeException msg)
            {
                //throw;
                return GenericResponses.ServiceUnavailable(msg.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel> GetAllComputersByUser(Guid userId, ClaimsPrincipal user)
        {
            if (!user.IsTheRequestedOneId(userId)) return GenericResponses.Forbidden();

            try
            {
                var res = await userRepository.GetAllComputersByUser(userId, x => x.MapObjectTo(new GetAllComputersByUserResponseModel()));
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
