using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Queries.RegionsQueries;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.RegionsHandlers
{
    public class GetAllRegionHandler : IRequestHandler<GetAllRegionQuery, List<RegionDto>>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public GetAllRegionHandler(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        public async Task<List<RegionDto>> Handle(GetAllRegionQuery request, CancellationToken cancellationToken)
        {
            var regionsDomainModel = await regionRepository.GetAllAsync();
            return mapper.Map<List<RegionDto>>(regionsDomainModel);
        }
    }
}
