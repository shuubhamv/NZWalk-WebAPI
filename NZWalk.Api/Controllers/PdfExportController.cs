using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.CQRS.Queries.RegionsQueries;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Services;

namespace NZWalk.Api.Controllers
{
    [Route("api/pdf")]
    [ApiController]
    public class PdfExportController : ControllerBase
    {
        private readonly IPdfGenerator pdfGenerator;
        private readonly IMediator mediator;
        private readonly ILogger<PdfExportController> logger;

        public PdfExportController(IPdfGenerator pdfGenerator, IMediator mediator, ILogger<PdfExportController> logger)
        {
            this.pdfGenerator = pdfGenerator;
            this.mediator = mediator;
            this.logger = logger;
        }

        // Generate Region Data PDF
        [HttpGet("regions")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GenerateRegionPdf()
        {
            try
            {
                logger.LogInformation("Generating PDF for all regions...");

                var query = new GetAllRegionQuery();
                var regions = await mediator.Send(query);

                if (regions == null || !regions.Any())
                    return NotFound("No region data found to export.");

                var pdfBytes = await pdfGenerator.GenerateRegionsPdfAsync(regions.ToList());
                return File(pdfBytes, "application/pdf", "RegionsReport.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to genrate Pdf");
                throw new Exception($"{ex.Message}");
            }
        }

        // Generate Custom Input PDF
        [HttpPost("custom")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GenerateCustomPdf([FromBody] CustomPdfRequestDto request)
        {
            try
            {
                logger.LogInformation($"Generating custom PDF with title: {request.Title}");

                var pdfBytes = await pdfGenerator.GenerateCustomPdfAsync(request);
                return File(pdfBytes, "application/pdf", "CustomInputReport.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to genrate Pdf");

                throw new Exception($"{ex.Message}");
            }
        }
    }
}
