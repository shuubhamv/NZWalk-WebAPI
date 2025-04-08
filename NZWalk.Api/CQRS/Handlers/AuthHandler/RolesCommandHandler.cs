using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalk.Api.CQRS.Queries.AuthQueries;

namespace NZWalk.Api.CQRS.Handlers.AuthHandler
{
    //public class RolesCommandHandler : IRequestHandler<GetRolesQuery, string>
    //{
    //    public RolesCommandHandler(RoleManager<IdentityRole> roleManager)
    //    {
    //        RoleManager = roleManager;
    //    }

    //    public RoleManager<IdentityRole> RoleManager { get; }

    //    public Task<string> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    //    {
    //        var roles = RoleManager.Roles.Select(r => r.Name).ToList();
    //        return null; 
    //    }
    //}
}
