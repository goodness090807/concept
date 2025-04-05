using System.Security.Claims;

namespace Concept.API.Extensions
{
    public static class AuthorizationExtension
    {
        /// <summary>
        /// 從 ClaimsPrincipal 中取得使用者 Id
        /// </summary>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return int.Parse(idClaim.Value);
        }
    }
}
