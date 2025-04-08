using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.RegionsCommands
{
    public class CreateRegionsCommand:IRequest<RegionDto>
    {
        public AddRegionRequestDto RegionRequestDto { get; set; }

        public CreateRegionsCommand(AddRegionRequestDto regionRequestDto)
        {
            RegionRequestDto = regionRequestDto;
        }
    }
}
