using System.Security.Claims;
using Domain.Exceptions;
using Domain.Results;
using Microsoft.AspNetCore.Authorization;
using Web.Authorization.Requirements;

namespace Web.Authorization.Handlers;

public class GuidClaimHandler : AuthorizationHandler<GuidClaimRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        GuidClaimRequirement requirement)
    {
        var user = context.User;
        var claim = user.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException
            {
                Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
            };
        if (!Guid.TryParse(claim.Value, out _))
        {
            throw new UnauthorizedException
            {
                Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
            };
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}