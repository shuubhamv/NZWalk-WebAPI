using MediatR;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.WalksCommands
{
    public class CreateWalkCommand : IRequest<WalkDto>
    {
        public AddWalkRequestDto WalkRequestDto { get; }

        public CreateWalkCommand(AddWalkRequestDto walkRequestDto)
        {
            WalkRequestDto = walkRequestDto;
        }


    }
}
