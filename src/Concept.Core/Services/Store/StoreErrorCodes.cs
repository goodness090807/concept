namespace Concept.Core.Services.Store
{
    public static class StoreErrorCodes
    {
        public const string UserAlreadyHasStore = "409001";
        public const string StoreNotFound = "404001";
        public const string InvalidUserId = "400001";
        public const string InvalidGrantedByUserId = "400002";
        public const string CannotGrantPermissionToSelf = "400003";
        public const string CannotGrantOwnerPermission = "400004";
        public const string InvalidData = "400005";
        public const string InsufficientPermission = "403001";
        public const string ResourceNotFound = "404002";
        public const string PermissionGrantFailed = "500001";
        public const string UpdateFailed = "500002";
        public const string UserNotFound = "404003";
        public const string PermissionAlreadyExists = "409002";
    }
}
