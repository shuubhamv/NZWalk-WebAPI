using MediatR;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.ImageCommands
{
    public class UploadImageCommand : IRequest<Image>
    {
        public ImageUploadRequestDto RequestDto { get; set; }
        public UploadImageCommand(ImageUploadRequestDto requestDto)
        {
            RequestDto = requestDto;
        }

    }
}
