using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.RegionsCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.RegionsHandlers
{
    public class DeleteRegionCommandHandler : IRequestHandler<DeleteReagionsCommand, RegionDto>
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public DeleteRegionCommandHandler(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        public async Task<RegionDto> Handle(DeleteReagionsCommand request, CancellationToken cancellationToken)
        {
            var deletedRegion = await regionRepository.DeleteAsync(request.Id);
            if (deletedRegion == null)
            { return null; } // Controller will handle NotFound
            return mapper.Map<RegionDto>(deletedRegion);
        }
    }
}
