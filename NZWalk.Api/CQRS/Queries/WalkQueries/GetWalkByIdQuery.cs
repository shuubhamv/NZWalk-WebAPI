using MediatR;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Queries.WalkQueries
{
    public class GetWalkByIdQuery : IRequest<WalkDto>
    {
        public Guid Id { get; }

        public GetWalkByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
