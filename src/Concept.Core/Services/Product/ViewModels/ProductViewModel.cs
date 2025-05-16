namespace Concept.Core.Services.Product.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(int id, string name, string description, decimal price, int stock, int storeId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            StoreId = storeId;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// 商品庫存
        /// </summary>
        public int Stock { get; }

        /// <summary>
        /// 商店ID
        /// </summary>
        public int StoreId { get; }
    }
}
