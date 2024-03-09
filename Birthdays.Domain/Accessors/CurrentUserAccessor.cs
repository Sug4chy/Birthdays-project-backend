using System.Security.Claims;
using Data.Entities;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Users;
using Microsoft.AspNetCore.Http;

namespace Domain.Accessors;

public class CurrentUserAccessor(
    IHttpContextAccessor accessor,
    IUserService userService) : ICurrentUserAccessor
{
    private readonly HttpContext _context = accessor.HttpContext!;
    
    public async Task<User> GetCurrentUserAsync(CancellationToken ct = default)
    {
        string userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        throw new UnauthorizedException
                        {
                            Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
                        };
        var currentUser = await userService.GetUserByIdAsync(userId, ct)
                          ?? throw new UnauthorizedException
                          {
                              Error = AuthErrors.InvalidAccessToken
                          };
        return currentUser;
    }
}