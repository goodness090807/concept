using Concept.Core.Common;

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
    }
}
