using System;

namespace Concept.Core.Services.Product.ViewModels
{
    /// <summary>
    /// 商品管理者可見的商店資訊視圖模型
    /// 僅包含商品管理所需的基本信息
    /// </summary>
    public class ProductManagerStoreViewModel
    {
        public ProductManagerStoreViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// 商店ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string Name { get; }
    }
}
