namespace Concept.Core.Services.Product
{
    public static class ProductErrorCodes
    {
        // 404 - Not Found
        public const string ProductNotFound = "404001";
        public const string StoreNotFound = "404002";
        public const string UserNotFound = "404003";
        public const string ResourceNotFound = "404004";
        
        // 403 - Forbidden
        public const string InsufficientPermission = "403001";
        public const string CannotGrantPermissionToSelf = "403002";
        public const string CannotGrantOwnerPermission = "403003";
        
        // 400 - Bad Request
        public const string InvalidPrice = "400001";
        public const string InvalidStock = "400002";
        public const string InvalidGrantedByUserId = "400003";
        public const string InvalidUserId = "400004";
        public const string PermissionAlreadyExists = "400005";
        
        // 500 - Server Errors
        public const string ProductAddFailed = "500001";
        public const string PermissionGrantFailed = "500002";
    }
}
