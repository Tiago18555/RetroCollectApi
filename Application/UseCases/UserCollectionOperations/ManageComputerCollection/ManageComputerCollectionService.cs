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

        public async Task<ResponseModel> AddComputer(AddItemRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            var user_id = requestToken.GetUserId();

            var user = userRepository.Any(u => u.UserId == user_id);
            if (!user) { return GenericResponses.NotFound("User not found"); }

            if (!computerRepository.Any(g => g.ComputerId == requestBody.Item_id))
            {
                try
                {
                    var result = await searchComputerService.RetrieveComputerInfoAsync(requestBody.Item_id);

                    var computerInfo = result.Single();

                    Computer computer = new()
                    {
                        ComputerId = computerInfo.ComputerId,
                        Description = computerInfo.Description,
                        ImageUrl = computerInfo.ImageUrl,
                        Name = computerInfo.Name,
                        IsArcade = computerInfo.IsArcade
                    };


                    var res = computerRepository.Add(computer);
                }
                catch (NullClaimException msg)
                {
                    return GenericResponses.BadRequest(msg.ToString());
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
                    ComputerId = requestBody.Item_id,
                    UserId = requestBody.User_id,
                    Condition = Enum.Parse<Condition>(requestBody.Condition.ToCapitalize(typeof(Condition))),
                    OwnershipStatus = Enum.Parse<OwnershipStatus>(requestBody.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                    Notes = requestBody.Notes == null ? null : requestBody.Notes,
                    PurchaseDate = requestBody.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : requestBody.PurchaseDate
                };

                var res = userComputerRepository.Add(userComputer);
                return res.MapObjectTo(new AddComputerResponseModel()).Created();
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

        public ResponseModel DeleteComputer(Guid user_computer_id, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();

                var foundItem = userComputerRepository.SingleOrDefault(r => r.UserId == user_id && r.UserComputerId == user_computer_id);
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
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
        }

        public async Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel requestBody, ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();

                var foundUser = userRepository.Any(x => x.UserId == user_id);
                if (!foundUser) { return GenericResponses.NotFound("User not found"); }

                var foundComputer = userComputerRepository.Any(x => x.UserComputerId == requestBody.UserComputerId);
                if (!foundComputer) { return GenericResponses.NotFound("Item Not Found"); }

                if (!computerRepository.Any(g => g.ComputerId == requestBody.Item_id) && requestBody.Item_id != 0)
                {
                    var result = await searchComputerService.RetrieveComputerInfoAsync(requestBody.Item_id);

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

                var res = this.userComputerRepository.Update(foundComputer.MapAndFill<UserComputer, UpdateComputerRequestModel>(requestBody));

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
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();
                var res = await userRepository.GetAllComputersByUser(user_id, x => x.MapObjectTo(new GetAllComputersByUserResponseModel()));
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
