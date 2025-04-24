using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.CQRS.Commands.ImageCommands;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using System;
using System.Threading.Tasks;
using Serilog;

namespace NZWalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IMediator mediator;
        private readonly ILogger<ImagesController> logger;

        public ImagesController(IImageRepository imageRepository,IMediator mediator, ILogger<ImagesController> logger)
        {
            this.imageRepository = imageRepository;
            this.mediator = mediator;
            this.logger = logger;
        }
        //post: api/images/upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request )
        {
            // ValidateFileUpload(request); 
            //if (ModelState.IsValid)
            //{
            //    // Convert DTO to domain model
            //    var imageDomainModel = new Image
            //    {
            //        File = request.File,     //That is  main file that we are going to upload
            //        FileExtension = Path.GetExtension(request.File.FileName),// automatically extract the extension of the file
            //        FileSizeInByte = request.File.Length,  // size of the file
            //        FileName = request.File.FileName,
            //        FileDescription = request.FileDescription
            //    };

            //    //Use repository to upload image

            //    await imageRepository.Upload(imageDomainModel);

            //    return Ok(imageDomainModel);

            //}
            //return BadRequest(ModelState);

            logger.LogInformation("[AuthController] Received Upload Image request.");
            try
            {
                if (request == null || request.File == null || request.File.Length == 0)
                {
                   logger.LogWarning("Invalid file upload attempt");
                    return BadRequest(new { Message = "Invalid file. Please upload a valid image." });
                }

                var command = new UploadImageCommand(request);



                logger.LogInformation("Image uploaded successfully: {FileName}", request.File.FileName);

                var image = await mediator.Send(command);


              
                // throw new Exception("This is new exception ");
                return Ok(image);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Image upload failed");
                return BadRequest(new { Message = "File upload failed", Error = ex.Message });
            }

        }



        //private void ValidateFileUpload(ImageUploadRequestDto request)
        //{
        //   var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //    if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
        //    {
        //        ModelState.AddModelError("file", "Invalid file type");
        //    }
        //    if (request.File.Length > 10485760)
        //    {
        //        ModelState.AddModelError("file", "File size should not exceed 10MB");
        //    }
        //}
    }
}
