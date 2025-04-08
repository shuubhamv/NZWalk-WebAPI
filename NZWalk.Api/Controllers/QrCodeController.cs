using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.CQRS.Commands.QrCodeCommands;
using NZWalk.Api.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;

namespace NZWalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Explicit scheme
    public class QrCodeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<QrCodeController> logger;

        public QrCodeController(IMediator mediator, UserManager<IdentityUser> userManager, ILogger<QrCodeController>logger)
        {
            _mediator = mediator;
            _userManager = userManager;
            this.logger = logger;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> GenerateQRCode([FromBody] QRCodeRequestDto request)
        {

            logger.LogInformation("[QRCodeController] Received GenerateQRCode Request.");
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email) ??
                        User.FindFirstValue(JwtRegisteredClaimNames.Email);

                if (string.IsNullOrEmpty(email))
                {
                    logger.LogWarning("Email not recevied");
                    return BadRequest("Email claim missing in token");
                }
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    logger.LogWarning("USer not found");
                    return Unauthorized("User not found");
                }

                var command = new GenerateQrCodeCommand(request, user.Id, user.Email);

                logger.LogInformation($"Finishe GenerateQrCodeCommand request with data:{JsonSerializer.Serialize(command)}");

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}