using CrossCutting;
using Application.UseCases.UserCollectionOperations.Shared;
using System.Security.Claims;

namespace Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public interface IManageConsoleCollectionUsecase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ResponseModel> AddConsole(AddItemRequestModel item, ClaimsPrincipal user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DeleteConsole(Guid id, ClaimsPrincipal user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel item, ClaimsPrincipal user);

        public Task<ResponseModel> GetAllConsolesByUser(ClaimsPrincipal user);

    }
}
