using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NZWalk.Api.Controllers;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XunitNZWalk.Test.Controllers
{

    public class ImagesControllerTests
    {
        private readonly Mock<IImageRepository> _mockRepo;
        private readonly ImagesController _controller;

        public ImagesControllerTests()
        {
            // Initialize mock repository
            _mockRepo = new Mock<IImageRepository>();
            // Inject mock repository into controller
            _controller = new ImagesController(_mockRepo.Object);
        }

        [Fact]
        public async Task Upload_ValidImage_ReturnsOkResult()
        {
            // Arrange: Create a mock file
            var fileMock = new Mock<IFormFile>();
            var content = new MemoryStream();
            var writer = new StreamWriter(content);
            writer.Write("dummy image content");
            writer.Flush();
            content.Position = 0;

            // Setup file properties
            fileMock.Setup(_ => _.OpenReadStream()).Returns(content);
            fileMock.Setup(_ => _.FileName).Returns("test.jpg");
            fileMock.Setup(_ => _.Length).Returns(content.Length);

            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Test Image"
            };

            var imageDomainModel = new Image
            {
                FileName = "test.jpg",
                FileExtension = ".jpg",
                FileSizeInByte = content.Length,
                FileDescription = "Test Image"
            };

            // Mock repository method to return expected result
            _mockRepo.Setup(repo => repo.Upload(It.IsAny<Image>())).ReturnsAsync(imageDomainModel);

            // Act: Call the Upload method
            var result = await _controller.Upload(request);

            // Assert: Verify the result is OkObjectResult with expected data
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Image>(okResult.Value);
            Assert.Equal("test.jpg", returnValue.FileName);
        }

        [Fact]
        public async Task Upload_InvalidFileExtension_ReturnsBadRequest()
        {
            // Arrange: Create a mock file with an invalid extension
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("test.txt"); // Invalid extension
            fileMock.Setup(_ => _.Length).Returns(100);

            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Invalid File"
            };

            // Act: Call the Upload method
            var result = await _controller.Upload(request);

            // Assert: Ensure the response is a BadRequestObjectResult
            Assert.IsType<BadRequestObjectResult>(result);
        }

        
        [Fact]
        public async Task Upload_FileTooLarge_ReturnsBadRequest()
        {
            // Arrange: Create a mock file larger than the allowed size (e.g., 5MB limit)
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("large.jpg");
            fileMock.Setup(x => x.Length).Returns(10885760); // MB file, exceeding limit

            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Too Large File"
            };

            // Act: Call the Upload method
            var result = await _controller.Upload(request);

            // Assert: Ensure the response is a BadRequestObjectResult
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

