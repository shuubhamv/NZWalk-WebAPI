namespace NZWalk.Api.Models.DTO
{
    public class CustomPdfRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }  // Needed for PDF generation
                                               //
    }
    }
