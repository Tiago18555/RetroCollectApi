using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public interface IManageConsoleCollectionService
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
        public Task<ResponseModel> UpdateGame(UpdateConsoleRequestModel item, ClaimsPrincipal user);
    }
}
