using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configs;
using Shared.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shared
{
    public class TokenService : ITokenService
    {
        private readonly TokenServiceConfig _tokenConfig;

        public TokenService(IOptions<TokenServiceConfig> options)
        {
            _tokenConfig = options.Value;
        }

        public string GenerateJwtToken(string userId, List<Claim>? claims = null, int tokenExpirationMinutes = 10)
        {
            var mergedClaims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId),
            };

            if (claims != null)
            {
                mergedClaims.AddRange(claims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: mergedClaims,
                expires: DateTimeOffset.UtcNow.DateTime.AddMinutes(tokenExpirationMinutes > 0 ? tokenExpirationMinutes : _tokenConfig.TokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = _tokenConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = _tokenConfig.Audience,
                ValidateLifetime = true,
            };
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
    }
}
