namespace NZWalk.Api.Models.DTO
{
    public class QRCodeRequestDto
    {
        public string Content { get; set; } // Custom content to encode (optional)
        public int? Size { get; set; } = 20;
    }
}
