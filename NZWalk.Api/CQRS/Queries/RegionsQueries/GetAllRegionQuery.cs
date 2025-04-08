using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Queries.RegionsQueries
{
    public class GetAllRegionQuery:IRequest<List<RegionDto>>
    {
    }
}
