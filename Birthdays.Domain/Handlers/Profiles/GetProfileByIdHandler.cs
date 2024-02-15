using System.Security.Claims;
using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetProfileByIdHandler(
    GetProfileByIdRequestValidator validator,
    IUserService userService,
    IProfileService profileService,
    IMapper mapper,
    ISubscriptionsService subscriptionsService,
    IHttpContextAccessor accessor,
    ILogger<GetProfileByIdHandler> logger)
{
    private readonly HttpContext _context = accessor.HttpContext!;

    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request,
        CancellationToken ct = default)
    {
        logger.LogInformation($"GetProfileById request was received for {request.UserId}'s profile");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        string userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new UnauthorizedException
                        {
                            Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
                        };

        var currentUser = await userService.GetUserByIdAsync(userId, ct) ??
                          throw new UnauthorizedException
                          {
                              Error = UsersErrors.NoSuchUserWithId(userId)
                          };

        var user = await userService.GetUserByIdAsync(request.UserId.ToString(), ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithId(request.UserId.ToString()));

        var profile = await profileService.GetProfileWithWishesByIdAsync(user!.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(user.ProfileId));

        logger.LogInformation($"GetProfileByUsernameResponse was successfully sent to {currentUser.Email}");
        return new GetProfileByIdResponse
        {
            Name = user.Name,
            Surname = user.Surname,
            Patronymic = user.Patronymic ?? "",
            Birthdate = user.BirthDate,
            Profile = mapper.Map<ProfileDto>(profile),
            IsCurrentUserSubscribedTo = await subscriptionsService
                .IsSubscribedToAsync(currentUser.ProfileId, profile!.Id, ct)
        };
    }
}