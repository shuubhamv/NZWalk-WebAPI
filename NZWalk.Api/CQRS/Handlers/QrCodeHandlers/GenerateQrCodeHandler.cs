using MediatR;
using NZWalk.Api.CQRS.Commands.QrCodeCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Services;

namespace NZWalk.Api.CQRS.Handlers.QrCodeHandlers
{
    public class GenerateQrCodeHandler:IRequestHandler<GenerateQrCodeCommand, QRCodeResponseDto>
    {
        private readonly IQrCodeService _qrCodeService;

        public GenerateQrCodeHandler(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        public async Task<QRCodeResponseDto> Handle(GenerateQrCodeCommand request, CancellationToken cancellationToken)
        {
            return await _qrCodeService.GenerateQRCodeAsync(
                           request.UserId,
                           request.UserEmail,
                           request.Request);
        }
    }
}
