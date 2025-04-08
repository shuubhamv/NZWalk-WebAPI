using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.WalksCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.WalksHandlers
{
    public class CreateWalkHandler: IRequestHandler<CreateWalkCommand,WalkDto>
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public CreateWalkHandler(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public async Task<WalkDto> Handle(CreateWalkCommand request, CancellationToken cancellationToken)
        {
            var walkDomainModel = mapper.Map<Walk>(request.WalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);
            return mapper.Map<WalkDto>(walkDomainModel);
        }
    }
}
