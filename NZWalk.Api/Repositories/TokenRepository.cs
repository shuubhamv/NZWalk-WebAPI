using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NZWalk.Api.Data;
using NZWalk.Api.Models.Domain;
using System.Threading.Tasks;


namespace NZWalk.Api.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly NZWalksAuthDbContext context;

        public TokenRepository(IConfiguration configuration,NZWalksAuthDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            //create claims
            try
            {
              var claims = new List<Claim>
                {
                    // Essential claims for user identification
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Maps to User.Identity.Name
                    new Claim(ClaimTypes.Email, user.Email) // Alternative email claim
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    claims.Add(new Claim("roles", role)); // Add this for better compatibility                }

                }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                          issuer: configuration["Jwt:Issuer"],
                           audience: configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(15),
                        signingCredentials: credentials
                        );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            catch (Exception )
            {

                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                var randomBytes = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }
                return Convert.ToBase64String(randomBytes);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            try
            {
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = userId,
                    ExpiryDate = DateTime.UtcNow.AddDays(7) // Refresh token valid for 7 days
                };

                context.RefreshTokens.Add(newRefreshToken);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string userId, string refreshToken)
        {
            try
            {
                return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked);

            }
            catch (Exception)
            {

                throw;
            }       
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                refreshToken.IsRevoked = true;
                context.RefreshTokens.Update(refreshToken);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


      
    }
}
