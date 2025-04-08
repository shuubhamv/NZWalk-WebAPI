using QRCoder;
using NZWalk.Api.Models.DTO;


namespace NZWalk.Api.Services
{
    public class QrCodeService : IQrCodeService
    {
        public Task<QRCodeResponseDto> GenerateQRCodeAsync(string userId, string userEmail, QRCodeRequestDto request)
        {
            try
            {
                // Combine user data with custom content
                var qrContent = string.IsNullOrEmpty(request.Content)
                    ? $"UserID:{userId}|Email:{userEmail}|Time:{DateTime.UtcNow:o}"
                    : $"UserID:{userId}|Email:{userEmail}|Custom:{request.Content}|Time:{DateTime.UtcNow:o}";

                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);

                var size = request.Size ?? 20; // Default size 20 if not specified
                var imageBytes = qrCode.GetGraphic(size);

                return Task.FromResult(new QRCodeResponseDto
                {
                    QRCodeImageBase64 = Convert.ToBase64String(imageBytes),
                    UserId = userId,
                    UserEmail = userEmail,
                    GeneratedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
              
                throw new ApplicationException($"{ex.Message}");
            }
        }
    }
}










//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using QRCoder;
//using NZWalk.Api.Models.DTO;

//namespace NZWalk.Api.Services
//{
//    public class QrCodeService : IQrCodeService
//    {
//        public Task<QRCodeResponseDto> GenerateQRCodeAsync(string userId, string userEmail, QRCodeRequestDto request)
//        { // Combine user data with custom content (if provided)
//            var qrContent = string.IsNullOrEmpty(request.Content)
//                ? $"UserID:{userId}|Email:{userEmail}|Time:{DateTime.UtcNow:o}"
//                : $"UserID:{userId}|Email:{userEmail}|Custom:{request.Content}|Time:{DateTime.UtcNow:o}";

//            using var qrGenerator = new QRCodeGenerator();
//            using var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
//            using var qrCode = new QRCode(qrCodeData);
//            using var qrCodeImage = qrCode.GetGraphic(20);

//            using var memoryStream = new MemoryStream();
//            qrCodeImage.Save(memoryStream, ImageFormat.Png);
//            var imageBytes = memoryStream.ToArray();

//            return new QRCodeResponseDto
//            {
//                QRCodeImageBase64 = Convert.ToBase64String(imageBytes),
//                UserId = userId,
//                UserEmail = userEmail,
//                GeneratedAt = DateTime.UtcNow
//            };
//        }
//    }
//}