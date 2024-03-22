using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SC_WEBAPISOCKET.Helpers
{
    public static class TokensHelpers
    {
        public static string GenerateJWTToken(string user)
        {
            var claims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, user),
    };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow.AddDays(-3),
                expires: DateTime.UtcNow.AddDays(-2),
                signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Config.JWT_Secret)
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public static ClaimsPrincipal ValidateToken(string jwtToken)
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;

                SecurityToken validatedToken;
                TokenValidationParameters validationParameters = new TokenValidationParameters();

                validationParameters.ValidateLifetime = true;

                validationParameters.ValidateAudience = false;
                validationParameters.ValidateIssuer = false;
                validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JWT_Secret));

                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

                return principal;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
