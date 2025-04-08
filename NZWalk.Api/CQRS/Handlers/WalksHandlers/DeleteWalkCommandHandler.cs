using AutoMapper;
using MediatR;
using NZWalk.Api.CQRS.Commands.WalksCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;

namespace NZWalk.Api.CQRS.Handlers.WalksHandlers
{
    public class DeleteWalkCommandHandler: IRequestHandler<DeleteWalkCommand, WalkDto>
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public DeleteWalkCommandHandler(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        public async Task<WalkDto> Handle(DeleteWalkCommand request, CancellationToken cancellationToken)
        {
            var deletedWalk = await walkRepository.DeleteAsync(request.Id);

            if (deletedWalk == null)
            { return null; } // Controller will handle NotFound

            return mapper.Map<WalkDto>(deletedWalk);
        }
    }
}
