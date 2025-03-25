using Shared.Interfaces;
using System.Security.Claims;

namespace Shared
{
    public class TokenService : ITokenService
    {
        public TokenService()
        {
            
        }

        public string GenerateJwtToken(string userId, List<Claim>? claims = null, int tokenExpirationMinutes = 10)
        {
            throw new NotImplementedException();
        }
    }
}
