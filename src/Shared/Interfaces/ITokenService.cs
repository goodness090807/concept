using System.Security.Claims;

namespace Shared.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// 產生 JWT Token (預設10分鐘過期)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claims"></param>
        /// <param name="tokenExpirationMinutes"></param>
        /// <returns></returns>
        string GenerateJwtToken(string userId, List<Claim>? claims = default, int tokenExpirationMinutes = 10);
    }
}
