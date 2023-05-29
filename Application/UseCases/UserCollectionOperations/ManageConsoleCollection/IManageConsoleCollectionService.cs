using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public interface IManageConsoleCollectionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ResponseModel> AddConsole(AddItemRequestModel item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DeleteConsole(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateGame(UpdateConsoleRequestModel item);
    }
}
