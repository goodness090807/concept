using System;

namespace Concept.Core.Services.Store.ViewModels
{
    /// <summary>
    /// 更新商店的ViewModel
    /// </summary>
    public class UpdateStoreViewModel
    {
        public UpdateStoreViewModel(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// 商店ID
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// 商店名稱
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 商店描述
        /// </summary>
        public string Description { get; }
    }
}
