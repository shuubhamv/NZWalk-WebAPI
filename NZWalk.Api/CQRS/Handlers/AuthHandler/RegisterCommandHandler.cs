using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalk.Api.CQRS.Commands.AuthCommands;

namespace NZWalk.Api.CQRS.Handlers.AuthHandler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<IdentityUser> userManager;

        public RegisterCommandHandler(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.RequestDto.Username,
                Email = request.RequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.RequestDto.Password);

            if (identityResult.Succeeded)
            {
                if (request.RequestDto.Roles != null && request.RequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.RequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return "User was registered! Please login.";
                    }
                }
            }

            return "Something went wrong during registration.";
        }
    }
    }

