using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace webApplication.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtUtils> _logger;

        public JwtUtils(
            IRepositoryWrapper wrapper,
            IOptions<AppSettings> appSettings,
            ILogger<JwtUtils> logger) 
        {
            _wrapper = wrapper;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public string GenerateJwtToken(user account)
        {
            //generate token thah is valid for 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new [] { new Claim("userid", account.userid.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                //token is a cryptographiacally strong random sequence of values
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                //token is valid for 7 days
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            //ensure token is unique by checking against db
            var tokenIsUnique = (await _wrapper.user.FindByCondition(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token))).Count == 0;

            if (!tokenIsUnique)
                return await GenerateRefreshToken(ipAddress);
            return refreshToken;
        }

        public int? ValidateJwtToken(string token)
        {
            if (token == null)
            {
                _logger.LogWarning("JWT token is null");
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "userid");
                if (userIdClaim == null)
                {
                    _logger.LogWarning("JWT missing 'userid' claim");
                    return null;
                }

                var accountId = int.Parse(userIdClaim.Value);
                _logger.LogInformation("JWT validated successfully for user ID: {UserId}", accountId);
                return accountId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT validation failed for token: {Token}", token.Substring(0, Math.Min(20, token.Length)) + "...");
                return null;
            }
        }
    }
}
