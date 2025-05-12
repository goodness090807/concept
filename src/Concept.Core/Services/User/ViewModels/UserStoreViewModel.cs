using Concept.Core.Entities.ResourceAuthorization.Enums;

namespace Concept.Core.Services.User.ViewModels
{
    /// <summary>
    /// 使用者的商店權限資訊
    /// </summary>
    public class UserStoreViewModel
    {
        public UserStoreViewModel(int id, string name, ResourcePermissionLevel permissionLevel, bool isOwner)
        {
            Id = id;
            Name = name;
            PermissionLevel = permissionLevel;
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
        /// 使用者對此商店的權限等級
        /// </summary>
        public ResourcePermissionLevel PermissionLevel { get; }

        /// <summary>
        /// 是否為此商店的擁有者
        /// </summary>
        public bool IsOwner { get; }
    }
}
