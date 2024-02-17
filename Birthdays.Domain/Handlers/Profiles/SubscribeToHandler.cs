using System.Security.Claims;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;
using Microsoft.AspNetCore.Http;

namespace Domain.Handlers.Profiles;

public class SubscribeToHandler(
    IHttpContextAccessor accessor,
    IUserService userService,
    SubscribeToRequestValidator validator,
    IProfileService profileService,
    ISubscriptionsService subscriptionsService)
{
    private readonly HttpContext _context = accessor.HttpContext!;

    public async Task<SubscribeToResponse> Handle(SubscribeToRequest request, CancellationToken ct = default)
    {
        string userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new UnauthorizedException
                        {
                            Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
                        };
        var currentUser = await userService.GetUserByIdAsync(userId, ct)
                          ?? throw new UnauthorizedException
                          {
                              Error = UsersErrors.NoSuchUserWithId(userId)
                          };
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);
        
        if (!await profileService.CheckIfUserExistsAsync(currentUser.ProfileId, ct))
        {
            throw new NotFoundException
            {
                Error = ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId)
            };
        }

        if (!await profileService.CheckIfUserExistsAsync(request.BirthdayManId, ct))
        {
            throw new NotFoundException
            {
                Error = ProfilesErrors.NoSuchProfileWithId(request.BirthdayManId)
            };
        }

        await subscriptionsService.SubscribeAsync(currentUser.ProfileId, request.BirthdayManId, ct);
        return new SubscribeToResponse();
    }
}