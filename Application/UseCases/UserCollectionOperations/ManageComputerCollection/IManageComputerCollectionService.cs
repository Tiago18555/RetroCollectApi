﻿using System.Security.Claims;
using Application.CrossCutting;
using Application.UseCases.UserCollectionOperations.Shared;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public interface IManageComputerCollectionService
    {
        public Task<ResponseModel> AddComputer(AddItemRequestModel item, ClaimsPrincipal user);
        public ResponseModel DeleteComputer(Guid id, ClaimsPrincipal user);
        public Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel item, ClaimsPrincipal user);
        public Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal user);

    }
}
