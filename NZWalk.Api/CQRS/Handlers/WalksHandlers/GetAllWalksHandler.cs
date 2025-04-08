using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Queries.WalkQueries;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.WalksHandlers
{
    public class GetAllWalksHandler:IRequestHandler<GetAllWalksQuery, IEnumerable<WalkDto>>
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public GetAllWalksHandler(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<WalkDto>> Handle(GetAllWalksQuery request, CancellationToken cancellationToken)
        {
            //var walks = await walkRepository.GetAllAsync();

            var walksDomainModel = await walkRepository.GetAllAsync(
             request.FilterOn,
             request.FilterQuery,
             request.SortBy,
             request.IsAscending ?? true,
             request.PageNumber,
             request.PageSize);

            return mapper.Map<IEnumerable<WalkDto>>(walksDomainModel);
        }
    }
}
