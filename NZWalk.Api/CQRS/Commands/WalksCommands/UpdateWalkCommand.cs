using MediatR;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.WalksCommands
{
    public class UpdateWalkCommand : IRequest<WalkDto>
    {
        public Guid Id { get; set; }
        public UpdateWalkRequestDto UpdateWalkRequestDto { get; set; }

        public UpdateWalkCommand(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            Id = id;
            UpdateWalkRequestDto = updateWalkRequestDto;
        }
    }
}

