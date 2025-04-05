namespace Concept.Core.Services.User
{
    public static class UserErrorCodes
    {
        public const string PasswordAndConfirmPasswordNotMatch = "400001";
        public const string InvalidPassword = "400002";

        public const string UserNotFound = "404001";

        public const string EmailAlreadyExists = "409001";
    }
}
