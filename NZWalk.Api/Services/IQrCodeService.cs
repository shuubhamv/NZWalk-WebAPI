using NZWalk.Api.Models.DTO;
namespace NZWalk.Api.Services
{
    public interface IQrCodeService
    {
        Task<QRCodeResponseDto> GenerateQRCodeAsync(string userId, string userEmail, QRCodeRequestDto request);
    }
}
