using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Queries.RegionsQueries
{
    public class GetAllRegionsByIdQuery:IRequest<RegionDto>
    {
        public Guid Id { get; set; }
        public GetAllRegionsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
