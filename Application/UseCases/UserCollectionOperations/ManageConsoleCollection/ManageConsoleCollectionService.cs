﻿using CrossCutting;
using Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using Application.UseCases.UserCollectionOperations.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Console = Domain.Entities.Console;

namespace Application.UseCases.UserCollectionOperations.ManageConsoleCollection
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


        public async Task<ResponseModel> AddConsole(AddItemRequestModel requestBody, ClaimsPrincipal requestToken)
        {
            var user_id = requestToken.GetUserId();

            var user = userRepository.Any(u => u.UserId == user_id);
            if (!user) { return GenericResponses.NotFound("User not found"); }

            if (!consoleRepository.Any(g => g.ConsoleId == requestBody.Item_id))
            {
                //Função adicionar console na entity Console=>
                var result = await searchConsoleService.RetrieveConsoleInfoAsync(requestBody.Item_id);

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
                    ConsoleId = requestBody.Item_id,
                    UserId = requestBody.User_id,
                    Condition = Enum.Parse<Condition>(requestBody.Condition.ToCapitalize(typeof(Condition))),
                    OwnershipStatus = Enum.Parse<OwnershipStatus>(requestBody.OwnershipStatus.ToCapitalize(typeof(OwnershipStatus))),
                    Notes = requestBody.Notes == null ? null : requestBody.Notes,
                    PurchaseDate = requestBody.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : requestBody.PurchaseDate
                };

                var res = userConsoleRepository.Add(userConsole);
                return res.MapObjectsTo(new AddConsoleResponseModel()).Created();
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
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }

        }
        public ResponseModel DeleteConsole(Guid user_console_id, ClaimsPrincipal requestToken)
        {
            try
            {
                var user_id = requestToken.GetUserId();

                var foundItem = userConsoleRepository.SingleOrDefault(r => r.UserId == user_id && r.UserConsoleId == user_console_id);
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
            catch (NullClaimException msg)
            {
                return GenericResponses.BadRequest(msg.ToString());
            }
        }

        public async Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel requestBody, ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();

                var foundUser = userRepository.Any(x => x.UserId == requestBody.User_id);
                if (!foundUser) { return GenericResponses.NotFound("User not found"); }

                var foundConsole = userConsoleRepository.Any(x => x.UserConsoleId == requestBody.UserConsoleId);
                if (!foundConsole) { return GenericResponses.NotFound("Item Not Found"); }

                if (!consoleRepository.Any(g => g.ConsoleId == requestBody.Item_id) && requestBody.Item_id != 0)
                {
                    var result = await searchConsoleService.RetrieveConsoleInfoAsync(requestBody.Item_id);

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

                var res = this.userConsoleRepository.Update(foundConsole.MapAndFill<UserConsole, UpdateConsoleRequestModel>(requestBody));

                return res.MapObjectsTo(new UpdateConsoleResponseModel()).Ok();
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

        public async Task<ResponseModel> GetAllConsolesByUser(ClaimsPrincipal requestToken)
        {

            try
            {
                var user_id = requestToken.GetUserId();
                var res = await userRepository.GetAllConsolesByUser(user_id, x => new UserConsole()
                {
                    UserConsoleId = x.UserConsoleId,
                    Console = x.Console,
                    Condition = x.Condition,
                    PurchaseDate = x.PurchaseDate,
                    Notes = x.Notes,
                    OwnershipStatus = x.OwnershipStatus
                });

                res.ForEach(x => x.MapObjectsTo(new GetAllConsolesByUserResponseModel()));
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
