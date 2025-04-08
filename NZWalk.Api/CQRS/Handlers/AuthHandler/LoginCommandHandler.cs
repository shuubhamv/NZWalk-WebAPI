using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalk.Api.CQRS.Commands.AuthCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.AuthHandler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        // Store refresh tokens (Use DB in production)
        private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();
        public LoginCommandHandler(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }



        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(request.RequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, request.RequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

                        var refreshToken = tokenRepository.GenerateRefreshToken();


                        // Store refresh token in database
                        await tokenRepository.SaveRefreshTokenAsync(user.Id, refreshToken);




                        return new LoginResponseDto { JwtToken = jwtToken, RefreshToken = refreshToken };
                    }
                }
            }

            return null;
        } 
    }
    }


    

