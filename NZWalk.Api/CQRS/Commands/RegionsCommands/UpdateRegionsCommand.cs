using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.RegionsCommands
{
    public class UpdateRegionsCommand: IRequest<RegionDto>
    {
        public Guid Id { get; set; }
        public UpdateRegionRequestDto RegionRequestDto { get; set; }
        public UpdateRegionsCommand(Guid id, UpdateRegionRequestDto regionRequestDto)
        {
            Id = id;
            RegionRequestDto = regionRequestDto;
        }
    }
}
