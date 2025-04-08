using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Queries.RegionsQueries;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.RegionsHandlers
{
    public class GetRegionByIdQueryHandler : IRequestHandler<GetAllRegionsByIdQuery, RegionDto>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public GetRegionByIdQueryHandler(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        public async Task<RegionDto> Handle(GetAllRegionsByIdQuery request, CancellationToken cancellationToken)
        {
            var regionDomainModel = await regionRepository.GetByIdAsync(request.Id);

            if (regionDomainModel == null)
            {
                return null;
            }
            return mapper.Map<RegionDto>(regionDomainModel);
        }
    }
}
