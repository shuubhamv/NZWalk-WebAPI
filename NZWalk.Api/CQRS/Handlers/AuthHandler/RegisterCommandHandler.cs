using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalk.Api.CQRS.Commands.AuthCommands;
using NZWalk.Api.Models.EmailConfiguration;
using NZWalk.Api.Services;
using NZWalk.Api.Services.EmailService;

namespace NZWalk.Api.CQRS.Handlers.AuthHandler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<RegisterCommandHandler> logger;
        private readonly IBackgroundEmailQueue emailQueue;

        public RegisterCommandHandler(UserManager<IdentityUser> userManager,
            ILogger<RegisterCommandHandler>logger,IBackgroundEmailQueue emailQueue)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.emailQueue = emailQueue;
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
                }

                    if (identityResult.Succeeded)
                    {
                        try
                        {
                            // Prepare welcome email content
                            var subject = "Welcome to NZWalks!";
                            var message = $"Hello {identityUser.UserName},<br><br>" +
                                         "Thank you for registering with NZWalks.<br>" +
                                         "We're excited to have you on board!<br><br>" +
                                         "Happy walking!<br>" +
                                         "The NZWalks Team";

                            // For IHostedService approach:
                            var emailWorkItem = new EmailWorkItem(
                                identityUser.Email,
                                subject,
                                message,
                                IsHtml: true);
                            await emailQueue.QueueBackgroundWorkItemAsync(emailWorkItem);

                            logger.LogInformation("Welcome email queued for {Email}", identityUser.Email);
                        }
                        catch (Exception ex)
                        {
                           logger.LogError(ex, "Failed to queue welcome email for {Email}", identityUser.Email);
                            // Don't fail registration if email fails
                        }


                        return "User was registered! Please login.";
                    }
                
            }

            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            logger.LogWarning("User registration failed: {Errors}", errors);
            return "Something went wrong during registration.";
        }
    }
    
}

