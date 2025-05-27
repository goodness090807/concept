namespace Concept.Core.Services.User.ViewModels
{
    /// <summary>
    /// 使用者的商店權限資訊
    /// </summary>
    public class UserStoreViewModel
    {
        public UserStoreViewModel(int id, string name, bool isOwner)
        {
            Id = id;
            Name = name;
            IsOwner = isOwner;
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
        /// 是否為此商店的擁有者
        /// </summary>
        public bool IsOwner { get; }
    }
}
