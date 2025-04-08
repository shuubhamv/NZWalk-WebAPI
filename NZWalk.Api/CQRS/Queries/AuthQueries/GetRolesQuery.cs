using MediatR;
using Microsoft.AspNetCore.Identity;

namespace NZWalk.Api.CQRS.Queries.AuthQueries
{
    public class GetRolesQuery: IRequest<List<string>>
    {
    }
}
