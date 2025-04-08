using MediatR;
using NZWalk.Api.CQRS.Commands.ImageCommands;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.ImageHandlers
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, Image>
    {
        private readonly IImageRepository imageRepository;

        public UploadImageCommandHandler(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public async Task<Image> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var imageDomainModel = new Image
            {
                File = request.RequestDto.File,
                FileExtension = Path.GetExtension(request.RequestDto.File.FileName),
                FileSizeInByte = request.RequestDto.File.Length,
                FileName = request.RequestDto.File.FileName,
                FileDescription = request.RequestDto.FileDescription
            };

            await imageRepository.Upload(imageDomainModel);
            return imageDomainModel;
        }
    }
}
