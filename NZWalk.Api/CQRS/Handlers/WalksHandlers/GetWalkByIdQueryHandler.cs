using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Queries.WalkQueries;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.WalksHandlers
{
    public class GetWalkByIdQueryHandler: IRequestHandler<GetWalkByIdQuery, WalkDto>
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public GetWalkByIdQueryHandler(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public async Task<WalkDto> Handle(GetWalkByIdQuery request, CancellationToken cancellationToken)
        {
            var walk = await walkRepository.GetByIdAsync(request.Id);
            return walk == null ? null : mapper.Map<WalkDto>(walk);
        }
    }
}
