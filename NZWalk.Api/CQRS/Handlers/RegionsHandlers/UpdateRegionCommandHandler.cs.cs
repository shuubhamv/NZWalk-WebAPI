using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.RegionsCommands;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.RegionsHandlers
{
    public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionsCommand, RegionDto>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public UpdateRegionCommandHandler(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        public async Task<RegionDto> Handle(UpdateRegionsCommand request, CancellationToken cancellationToken)
        {
            var regionDomainModel = mapper.Map<Region>(request.RegionRequestDto);

            var updatedRegion = await regionRepository.UpdateAsync(request.Id,regionDomainModel);

            if (updatedRegion == null)
            { return null; }

            return mapper.Map<RegionDto>(updatedRegion);


        }
    }
}
