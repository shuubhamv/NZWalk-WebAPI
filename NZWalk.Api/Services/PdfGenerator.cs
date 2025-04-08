using NZWalk.Api.Models.DTO;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NZWalk.Api.Services
{
    public class PdfGenerator : IPdfGenerator
    {
        public async Task<byte[]> GenerateCustomPdfAsync(CustomPdfRequestDto request)
        {
            using var doc = new PdfDocument();

            // Adds a new page to the PDF
            var page = doc.AddPage();

            // Allows us to draw (text, images) on that page.
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 14, XFontStyle.Regular);

            // Draws title and description
            gfx.DrawString($"Title: {request.Title}", font, XBrushes.Black, new XRect(20, 40, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString($"Description: {request.Description}", font, XBrushes.Black, new XRect(20, 70, page.Width, page.Height), XStringFormats.TopLeft);

            // Check if Image URL is provided and starts with http
            if (!string.IsNullOrEmpty(request.ImageUrl) && request.ImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using var client = new HttpClient();

                    // Downloads the image from the URL
                    var stream = await client.GetStreamAsync(request.ImageUrl);

                    // Loads and resizes the image
                    using var image = await Image.LoadAsync<Rgba32>(stream);
                    image.Mutate(x => x.Resize(200, 100));

                    // Converts image to stream, then draws on PDF at (20, 130)
                    using var ms = new MemoryStream();
                    await image.SaveAsync(ms, new PngEncoder());
                    ms.Position = 0;

                    var xImage = XImage.FromStream(() => ms);
                    gfx.DrawImage(xImage, 20, 130);
                }
                catch (Exception ex)
                {
                    // If image fails to load, show error message in red text
                    gfx.DrawString($"Image failed to load: {ex.Message}", font, XBrushes.Red, new XRect(20, 130, page.Width, page.Height), XStringFormats.TopLeft);
                }
            }
            else
            {
                // No valid image URL provided
                gfx.DrawString("No image available", font, XBrushes.Gray, new XRect(20, 130, page.Width, page.Height), XStringFormats.TopLeft);
            }

            // Saves the PDF into memory, returns it as byte array to download/send.
            using var outStream = new MemoryStream();
            doc.Save(outStream, false);
            return outStream.ToArray();
        }

        public async Task<byte[]> GenerateRegionsPdfAsync(List<RegionDto> regions)
        {
            using var doc = new PdfDocument();

            foreach (var region in regions)
            {
                var page = doc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 14, XFontStyle.Regular);

                gfx.DrawString($"Region Name: {region.Name}", font, XBrushes.Black, new XRect(20, 40, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Code: {region.Code}", font, XBrushes.Black, new XRect(20, 70, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Image URL: {region.RegionImageUrl}", font, XBrushes.Black, new XRect(20, 100, page.Width, page.Height), XStringFormats.TopLeft);

                if (!string.IsNullOrEmpty(region.RegionImageUrl) && region.RegionImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        using var client = new HttpClient();
                        var stream = await client.GetStreamAsync(region.RegionImageUrl);

                        using var image = await Image.LoadAsync<Rgba32>(stream);
                        image.Mutate(x => x.Resize(200, 100));

                        using var ms = new MemoryStream();
                        await image.SaveAsync(ms, new PngEncoder());
                        ms.Position = 0;

                        var xImage = XImage.FromStream(() => ms);
                        gfx.DrawImage(xImage, 20, 130);
                    }
                    catch (Exception ex)
                    {
                        gfx.DrawString($"Image failed to load: {ex.Message}", font, XBrushes.Red, new XRect(20, 160, page.Width, page.Height), XStringFormats.TopLeft);
                    }
                }
                else
                {
                    gfx.DrawString("No image available", font, XBrushes.Gray, new XRect(20, 130, page.Width, page.Height), XStringFormats.TopLeft);
                }
            }

            using var outStream = new MemoryStream();
            doc.Save(outStream, false);
            return outStream.ToArray();
        }
    }
}
