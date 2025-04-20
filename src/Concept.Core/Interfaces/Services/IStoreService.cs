using Concept.Core.Common;
using Concept.Core.Services.Store.ViewModels;

namespace Concept.Core.Interfaces.Services
{
    public interface IStoreService : IBaseService
    {
        /// <summary>
        /// 新增商店
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Result<int>> AddStoreAsync(int userId, string name);

        /// <summary>
        /// 取得商店資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<StoreViewModel>> GetStoreByIdAsync(int storeId);
    }
}
