using Concept.Core.Entities.Store;

namespace Concept.Core.Entities.Product
{
    public class ProductEntity : AuditableEntity
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品庫存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 商店ID
        /// </summary>
        public int StoreId { get; set; }
        
        /// <summary>
        /// 商店
        /// </summary>
        public StoreEntity Store { get; set; } = null!;
    }
}
