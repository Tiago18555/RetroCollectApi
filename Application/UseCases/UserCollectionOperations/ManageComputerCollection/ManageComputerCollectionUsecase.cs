using Domain;
using Application.UseCases.IgdbIntegrationOperations.SearchComputer;
using Application.UseCases.UserCollectionOperations.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public class ManageComputerCollectionService : IManageComputerCollectionUsecase
    {
        private readonly IUserRepository _userRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly IUserComputerRepository _userComputerRepository;
        private readonly ISearchComputerUsecase _searchComputerService;
        public ManageComputerCollectionService(
            IUserRepository userRepository,
            IComputerRepository computerRepository,
            IUserComputerRepository userComputerRepository,
            ISearchComputerUsecase searchComputerService
        )
        {
            this._userRepository = userRepository;
            this._computerRepository = computerRepository;
            this._userComputerRepository = userComputerRepository;
            this._searchComputerService = searchComputerService;
        }

        public async Task<ResponseModel> AddComputer(AddItemRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            var user_id = requestToken.GetUserId();

            var user = _userRepository.Any(u => u.UserId == user_id);
            if (!user) { return ResponseFactory.NotFound("User not found"); }

            if (!_computerRepository.Any(g => g.ComputerId == requestBody.Item_id))
            {
                try
                {
                    var result = await _searchComputerService.RetrieveComputerInfoAsync(requestBody.Item_id);

                    var computerInfo = result.Single();

                    Computer computer = new()
                    {
                        ComputerId = computerInfo.ComputerId,
                        Description = computerInfo.Description,
                        ImageUrl = computerInfo.ImageUrl,
                        Name = computerInfo.Name,
                        IsArcade = computerInfo.IsArcade
                    };


                    var res = _computerRepository.Add(computer);
                }
                catch (NullClaimException msg)
                {
                    return ResponseFactory.BadRequest(msg.ToString());
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

                var res = _userComputerRepository.Add(userComputer);
                return res.MapObjectsTo(new AddComputerResponseModel()).Created();
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
                return ResponseFactory.UnsupportedMediaType("Invalid type for Condition or OwnershipStatus: " + msg);
            }
            catch (InvalidEnumValueException msg)
            {
                return ResponseFactory.BadRequest("Invalid value for Condition or OwnershipStatus: " + msg);
            }
        }

        public ResponseModel DeleteComputer(Guid user_computer_id, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();

                var foundItem = _userComputerRepository.SingleOrDefault(r => r.UserId == user_id && r.UserComputerId == user_computer_id);
                if (foundItem == null) { return ResponseFactory.NotFound(); }

                if (_userComputerRepository.Delete(foundItem))
                {
                    return ResponseFactory.Ok("Computer deleted");
                }
                else
                {
                    return ResponseFactory.Ok("Not deleted");
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (NullClaimException msg)
            {
                return ResponseFactory.BadRequest(msg.ToString());
            }
        }

        public async Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel requestBody, ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();

                var foundUser = _userRepository.Any(x => x.UserId == user_id);
                if (!foundUser) { return ResponseFactory.NotFound("User not found"); }

                var foundComputer = _userComputerRepository.Any(x => x.UserComputerId == requestBody.UserComputerId);
                if (!foundComputer) { return ResponseFactory.NotFound("Item Not Found"); }

                if (!_computerRepository.Any(g => g.ComputerId == requestBody.Item_id) && requestBody.Item_id != 0)
                {
                    var result = await _searchComputerService.RetrieveComputerInfoAsync(requestBody.Item_id);

                    var computerInfo = result.Single();

                    Computer computer = new()
                    {
                        ComputerId = computerInfo.ComputerId,
                        Description = computerInfo.Description,
                        ImageUrl = computerInfo.ImageUrl,
                        Name = computerInfo.Name,
                        IsArcade = computerInfo.IsArcade
                    };
                    _computerRepository.Add(computer);

                }

                var res = this._userComputerRepository.Update(foundComputer.MapAndFill<UserComputer, UpdateComputerRequestModel>(requestBody));

                return res.MapObjectsTo(new UpdateComputerResponseModel()).Ok();
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
                return ResponseFactory.ServiceUnavailable(msg.ToString());
            }
            catch (NullClaimException msg)
            {
                return ResponseFactory.BadRequest(msg.ToString());
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
                var res = await _userRepository.GetAllComputersByUser(user_id, x => new UserComputer()
                {
                    UserComputerId = x.UserComputerId,
                    Computer = x.Computer,
                    Condition = x.Condition,
                    PurchaseDate = x.PurchaseDate,
                    Notes = x.Notes,
                    OwnershipStatus = x.OwnershipStatus
                });

                res.ForEach(x => x.MapObjectsTo(new GetAllComputersByUserResponseModel()));

                return res.Ok();
            }
            catch (ArgumentNullException)
            {
                throw;
                //return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (NullClaimException msg)
            {
                return ResponseFactory.BadRequest(msg.ToString());
            }
        }
    }
}
