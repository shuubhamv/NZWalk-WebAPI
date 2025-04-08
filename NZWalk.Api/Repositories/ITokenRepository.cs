using Microsoft.AspNetCore.Identity;
using NZWalk.Api.Models.Domain;

namespace NZWalk.Api.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user,List<string> roles);
        //
        string GenerateRefreshToken();
        
        // New methods for handling refresh tokens in DB
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string userId, string refreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken);
    }
}
