using MediatR;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using System.Globalization;

namespace NZWalk.Api.CQRS.Queries.WalkQueries
{
    public class GetAllWalksQuery : IRequest<IEnumerable<WalkDto>>
    {
        public string? FilterOn { get; }
        public string? FilterQuery { get; }
        public string? SortBy { get; }
        public bool? IsAscending { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetAllWalksQuery(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending, int pageNumber, int pageSize)
        {
            FilterOn = filterOn;
            FilterQuery = filterQuery;
            SortBy = sortBy;
            IsAscending = isAscending ?? true;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

    }
}
