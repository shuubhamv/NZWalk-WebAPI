using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.WalksCommands
{
    public class DeleteWalkCommand : IRequest<WalkDto>
    {
        public Guid Id { get; set; }

        public DeleteWalkCommand(Guid id)
        {
            Id = id;
        }
    }
}
