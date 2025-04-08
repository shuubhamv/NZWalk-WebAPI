using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.WalksCommands;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.WalksHandlers
{
    public class UpdateWalkCommandHandler: IRequestHandler<UpdateWalkCommand, WalkDto>
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public UpdateWalkCommandHandler(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        public async Task<WalkDto> Handle(UpdateWalkCommand request, CancellationToken cancellationToken)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(request.UpdateWalkRequestDto);

            // Update in database
            var updatedWalk = await walkRepository.UpdateAsync(request.Id, walkDomainModel);

            if (updatedWalk == null)
            { return null; } // Controller will handle NotFound

            return mapper.Map<WalkDto>(updatedWalk);
        }
    }
}
