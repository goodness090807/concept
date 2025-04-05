using Concept.Core.Entities.User.Enums;

namespace Concept.Core.Entities.User
{
    public class UserEntity : AuditableEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash過的密碼
        /// </summary>
        public string HashedPassword { get; set; } = string.Empty;

        /// <summary>
        /// 使用者狀態
        /// </summary>
        public UserStatus UserStatus { get; set; }
    }
}
