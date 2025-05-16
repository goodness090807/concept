using System.ComponentModel.DataAnnotations;

namespace Concept.API.Controllers.Store.Requests
{
    /// <summary>
    /// 更新商店請求
    /// </summary>
    public class UpdateStoreRequest
    {
        /// <summary>
        /// 商店名稱
        /// </summary>
        [Required(ErrorMessage = "商店名稱不能為空")]
        [StringLength(50, ErrorMessage = "商店名稱不能超過50個字元")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 商店描述
        /// </summary>
        [StringLength(1000, ErrorMessage = "商店描述不能超過1000個字元")]
        public string Description { get; set; } = string.Empty;
    }
}
