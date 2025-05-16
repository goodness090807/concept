using Concept.Core.Entities.Product;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="name">商品名稱</param>
        /// <param name="description">商品描述</param>
        /// <param name="price">商品價格</param>
        /// <param name="stock">商品庫存</param>
        /// <returns>新增的商品ID</returns>
        Task<int> AddProductAsync(int storeId, string name, string description, decimal price, int stock);
        
        /// <summary>
        /// 取得商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品實體</returns>
        Task<ProductEntity?> GetProductByIdAsync(int productId);
        
        /// <summary>
        /// 取得商店所有商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <returns>商品列表</returns>
        Task<List<ProductEntity>> GetProductsByStoreIdAsync(int storeId);
    }
}
