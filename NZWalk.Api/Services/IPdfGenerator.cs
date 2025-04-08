using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.Services
{
    public interface IPdfGenerator
    {
        Task<byte[]> GenerateRegionsPdfAsync(List<RegionDto> regions);
        Task<byte[]> GenerateCustomPdfAsync(CustomPdfRequestDto request);
    }
}
