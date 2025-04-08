using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalk.Api.CQRS.Commands.AuthCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;



namespace NZWalk.Api.CQRS.Handlers.AuthHandler
{
  
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();

        public RefreshTokenCommandHandler(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the refresh token from the database
            var storedRefreshToken = await tokenRepository.GetRefreshTokenAsync(request.RequestDto.UserId, request.RequestDto.RefreshToken);

            // Check if refresh token exists, is not expired, and is not revoked
            if (storedRefreshToken == null || storedRefreshToken.ExpiryDate < DateTime.UtcNow || storedRefreshToken.IsRevoked)
            {
                return null; // Return null or throw an Unauthorized error
            }

            // Generate new tokens
            var user = await userManager.FindByIdAsync(request.RequestDto.UserId);
            if (user == null)
            {
                return null;
            }
            
            var roles = await userManager.GetRolesAsync(user);
            var newAccessToken = tokenRepository.CreateJwtToken(user, roles.ToList());
            var newRefreshToken = tokenRepository.GenerateRefreshToken();

            // Revoke the old refresh token
            await tokenRepository.RevokeRefreshTokenAsync(storedRefreshToken);

            // Save the new refresh token
            await tokenRepository.SaveRefreshTokenAsync(user.Id, newRefreshToken);

            return new LoginResponseDto
            {
                JwtToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}