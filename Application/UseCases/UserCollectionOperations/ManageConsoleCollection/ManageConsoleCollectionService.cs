﻿using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.IgdbIntegrationOperations.SearchConsole;
using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using RetroCollectApi.Repositories.Interfaces;
using System.Data;
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


        public async Task<ResponseModel> AddConsole(AddItemRequestModel item)
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

            UserConsole userConsole = new()
            {
                ConsoleId = item.Item_id,
                UserId = item.User_id,
                Condition = (Condition)Enum.Parse(typeof(Condition), item.Condition),
                OwnershipStatus = (OwnershipStatus)Enum.Parse(typeof(OwnershipStatus), item.OwnershipStatus),
                Notes = item.Notes == null ? null : item.Notes,
                PurchaseDate = item.PurchaseDate == DateTime.MinValue ? DateTime.MinValue : item.PurchaseDate
            };

            try
            {
                var res = userConsoleRepository.Add(userConsole);
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
        public Task<ResponseModel> DeleteConsole()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> UpdateGame(UpdateConsoleRequestModel item)
        {
            throw new NotImplementedException();
        }
    }
}
