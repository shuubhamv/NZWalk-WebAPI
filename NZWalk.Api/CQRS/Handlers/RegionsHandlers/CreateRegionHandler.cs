using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.RegionsCommands;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.RegionsHandlers
{
    public class CreateRegionHandler : IRequestHandler<CreateRegionsCommand, RegionDto>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public CreateRegionHandler(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        public async Task<RegionDto> Handle(CreateRegionsCommand request, CancellationToken cancellationToken)
        {
            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(request.RegionRequestDto);
            // Create in database
            await regionRepository.CreateAsync(regionDomainModel);

            return mapper.Map<RegionDto>(regionDomainModel);


        }
    }
}
