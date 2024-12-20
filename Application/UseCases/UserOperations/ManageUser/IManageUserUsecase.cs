﻿using Domain;
using System.Security.Claims;

namespace Application.UseCases.UserOperations.ManageUser;

public interface IManageUserUsecase
{
    Task<ResponseModel> UpdateUser(UpdateUserRequestModel user, ClaimsPrincipal claim);
    Task<ResponseModel> GetAllUsers();
}

