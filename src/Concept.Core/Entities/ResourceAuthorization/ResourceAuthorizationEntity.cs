using Concept.Core.Entities.Resource;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.ResourceAuthorization
{
    public class ResourceAuthorizationEntity : AuditableEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 資源Id
        /// </summary>
        public int ResourceId { get; set; }
        public ResourceEntity Resource { get; set; } = null!;

        /// <summary>
        /// 被授權者
        /// </summary>
        public int AuthorizedUserId { get; set; }
        public UserEntity AuthorizedUser { get; set; } = null!;

        /// <summary>
        /// 授權者
        /// </summary>
        public int GrantedByUserId { get; set; }

        public UserEntity GrantedByUser { get; set; } = null!;

        /// <summary>
        /// 可設定權限過期時間
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 授權的具體權限
        /// </summary>
        public ResourcePermissionLevel PermissionLevel { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
