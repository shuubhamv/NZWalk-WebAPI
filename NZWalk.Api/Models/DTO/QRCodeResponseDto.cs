namespace NZWalk.Api.Models.DTO
{
    public class QRCodeResponseDto
    {
        public string QRCodeImageBase64 { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
