using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.RegionsCommands
{
    public class DeleteReagionsCommand:IRequest<RegionDto>
    {
        public Guid Id { get; set; }
        public DeleteReagionsCommand(Guid id)
        {
            Id = id;
        }
    }
}
