using Microsoft.AspNetCore.Http; // Provides HTTP-related functionality, including file uploads
using Microsoft.AspNetCore.Mvc; // Required for API controller functionalities
using Moq; // Moq framework for mocking dependencies in unit tests
using NZWalk.Api.Controllers; // Reference to the controller being tested
using NZWalk.Api.Models.Domain; // Domain model for images
using NZWalk.Api.Models.DTO; // DTO model for image upload requests
using NZWalk.Api.Repositories; // Image repository interface


// Namespace for the test class
namespace XunitNZWalk.Test.Controllers
{
    // Test class for ImagesController
    public class ImagesControllerTests
    {
        // Mock instance of IImageRepository for dependency injection
        private readonly Mock<IImageRepository> _mockRepo;

        // Instance of ImagesController to test its methods
        private readonly ImagesController _controller;

        // Constructor to initialize mock repository and controller
        public ImagesControllerTests()
        {
            // Create a mock object for IImageRepository
            _mockRepo = new Mock<IImageRepository>();

            // Inject the mock repository into the ImagesController
            //_controller = new ImagesController(_mockRepo.Object);
        }

        // Test case: Upload a valid image should return 200 OK response
        [Fact] // Marks this method as a unit test
        public async Task Upload_ValidImage_ReturnsOkResult()
        {
            // Arrange: Create a mock file to simulate image upload
            var fileMock = new Mock<IFormFile>();
            var content = new MemoryStream(); // Create an in-memory stream
            var writer = new StreamWriter(content);
            writer.Write("dummy image content"); // Write dummy data to the stream
            writer.Flush();
            content.Position = 0; // Reset the stream position to the beginning

            // Setup the mock file's properties
            fileMock.Setup(f => f.OpenReadStream()).Returns(content); // Return file stream
            fileMock.Setup(f => f.FileName).Returns("test.jpg"); // Set file name
            fileMock.Setup(f => f.Length).Returns(content.Length); // Set file size

            // Create a DTO request with the mock file
            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Test Image"
            };

            // Create a domain model that represents the uploaded image
            var imageDomainModel = new Image
            {
                FileName = "test.jpg",
                FileExtension = ".jpg",
                FileSizeInByte = content.Length,
                FileDescription = "Test Image"
            };

            // Mock the repository method to return the expected result
            _mockRepo.Setup(repo => repo.Upload(It.IsAny<Image>())).ReturnsAsync(imageDomainModel);

            // Act: Call the Upload method on the controller
            var result = await _controller.Upload(request);

            // Assert: Verify that the result is a 200 OK response with expected data
            var okResult = Assert.IsType<OkObjectResult>(result); // Check response type
            var returnValue = Assert.IsType<Image>(okResult.Value); // Check return type
            Assert.Equal("test.jpg", returnValue.FileName); // Ensure file name matches
        }

        // Test case: Upload a file with an invalid extension should return 400 Bad Request
        [Fact]
        public async Task Upload_InvalidFileExtension_ReturnsBadRequest()
        {
            // Arrange: Create a mock file with an invalid extension (.txt)
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.txt"); // Invalid extension
            fileMock.Setup(f => f.Length).Returns(100); // Small valid size

            // Create DTO request with the invalid file
            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Invalid File"
            };

            // Act: Call the Upload method
            var result = await _controller.Upload(request);

            // Assert: Verify the response is 400 Bad Request
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test case: Upload a file that exceeds the size limit should return 400 Bad Request
        [Fact]
        public async Task Upload_FileTooLarge_ReturnsBadRequest()
        {
            // Arrange: Create a mock file larger than the allowed size (10MB limit)
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("large.jpg");
            fileMock.Setup(x => x.Length).Returns(10885760); // Exceeds 10MB

            // Create DTO request with the oversized file
            var request = new ImageUploadRequestDto
            {
                File = fileMock.Object,
                FileDescription = "Too Large File"
            };

            // Act: Call the Upload method
            var result = await _controller.Upload(request);

            // Assert: Verify the response is 400 Bad Request
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
