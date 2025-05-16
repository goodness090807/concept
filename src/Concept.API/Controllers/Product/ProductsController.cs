using Concept.API.Authorizations.ResourceAccess;
using Concept.API.Controllers.Product.Requests;
using Concept.API.Extensions;
using Concept.Core.Entities.ResourceAuthorization.Enums;
using Concept.Core.Interfaces.Services;
using Concept.Core.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concept.API.Controllers.Product
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <param name="req">商品資訊</param>
        /// <returns>商品ID</returns>
        [HttpPost("stores/{storeId}/products"), Authorize]
        [ResourceAccess("Store", "storeId", ResourcePermissionLevel.PRODUCT_MANAGER)]
        public async Task<IActionResult> AddProductAsync(int storeId, [FromBody] AddProductRequest req)
        {
            var result = await _productService.AddProductAsync(
                storeId,
                User.GetUserId(),
                req.Name,
                req.Description,
                req.Price,
                req.Stock);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ProductErrorCodes.StoreNotFound => NotFound("商店不存在"),
                    ProductErrorCodes.InsufficientPermission => Forbid("您沒有權限在此商店新增商品"),
                    ProductErrorCodes.InvalidPrice => BadRequest("商品價格不得為負數"),
                    ProductErrorCodes.InvalidStock => BadRequest("商品庫存不得為負數"),
                    _ => BadRequest("新增商品失敗")
                };
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// 取得商品資訊
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品資訊</returns>
        [HttpGet("products/{productId}"), Authorize]
        [ResourceAccess("Product", "productId", ResourcePermissionLevel.VIEWER)]
        public async Task<IActionResult> GetProductByIdAsync(int productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ProductErrorCodes.ProductNotFound => NotFound("商品不存在"),
                    _ => BadRequest("取得商品失敗")
                };
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// 取得商店的所有商品
        /// </summary>
        /// <param name="storeId">商店ID</param>
        /// <returns>商品列表</returns>
        [HttpGet("stores/{storeId}/products"), Authorize]
        [ResourceAccess("Store", "storeId", ResourcePermissionLevel.VIEWER)]
        public async Task<IActionResult> GetProductsByStoreIdAsync(int storeId)
        {
            var result = await _productService.GetProductsByStoreIdAsync(storeId);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ProductErrorCodes.StoreNotFound => NotFound("商店不存在"),
                    _ => BadRequest("取得商品列表失敗")
                };
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// 授予商品權限
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="req">權限設置</param>
        /// <returns>授權ID</returns>
        [HttpPost("products/{productId}/permissions"), Authorize]
        [ResourceAccess("Product", "productId", ResourcePermissionLevel.PRODUCT_MANAGER)]
        public async Task<IActionResult> GrantProductPermissionAsync(int productId, [FromBody] GrantProductPermissionRequest req)
        {
            var result = await _productService.GrantProductPermissionAsync(
                productId,
                User.GetUserId(),
                req.UserId,
                req.PermissionLevel,
                req.ExpiresAt);

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ProductErrorCodes.ProductNotFound => NotFound("商品不存在"),
                    ProductErrorCodes.UserNotFound => NotFound("被授權的使用者不存在"),
                    ProductErrorCodes.ResourceNotFound => NotFound("找不到商品對應的資源"),
                    ProductErrorCodes.InsufficientPermission => Forbid("您沒有足夠的權限進行授權"),
                    ProductErrorCodes.InvalidUserId => BadRequest("被授權使用者ID無效"),
                    ProductErrorCodes.CannotGrantPermissionToSelf => BadRequest("不能授予自己權限"),
                    ProductErrorCodes.CannotGrantOwnerPermission => BadRequest("不能授予所有者權限"),
                    ProductErrorCodes.PermissionAlreadyExists => BadRequest("該用戶已經擁有此商品的權限"),
                    _ => BadRequest("授予權限失敗")
                };
            }

            return Ok(new { AuthorizationId = result.Data });
        }

        /// <summary>
        /// 取得商品所屬商店的基本信息
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商店基本信息</returns>
        [HttpGet("products/{productId}/store-info"), Authorize]
        [ResourceAccess("Product", "productId", ResourcePermissionLevel.VIEWER)]
        public async Task<IActionResult> GetProductStoreInfoAsync(int productId)
        {
            var result = await _productService.GetProductStoreBasicInfoAsync(
                productId,
                User.GetUserId());

            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ProductErrorCodes.ProductNotFound => NotFound("商品不存在"),
                    ProductErrorCodes.StoreNotFound => NotFound("找不到商品所屬的商店"),
                    ProductErrorCodes.InsufficientPermission => Forbid("您沒有權限查看此商品信息"),
                    _ => BadRequest("獲取商店信息失敗")
                };
            }

            return Ok(result.Data);
        }
    }
}
