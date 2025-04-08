using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.QrCodeCommands
{
    public class GenerateQrCodeCommand:IRequest<QRCodeResponseDto>
    {
        public QRCodeRequestDto Request { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public GenerateQrCodeCommand(QRCodeRequestDto request, string userId, string userEmail)
        {
            Request = request;
            UserId = userId;
            UserEmail = userEmail;
        }
    }
}
