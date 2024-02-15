using System.Security.Claims;
using AutoMapper;
using Domain.DTO;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetCurrentProfileHandler(
    IUserService userService,
    IProfileService profileService,
    IMapper mapper,
    IHttpContextAccessor accessor,
    ILogger<GetCurrentProfileHandler> logger)
{
    private readonly HttpContext _context = accessor.HttpContext!;

    public async Task<GetCurrentProfileResponse> Handle(CancellationToken ct = default)
    {
        string userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            throw new UnauthorizedException
            {
                Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
            };
        logger.LogInformation($"GetCurrentProfile request was received from user with id {userId}");
        var currentUser = await userService.GetUserByIdAsync(userId, ct)
                          ?? throw new UnauthorizedException
                          {
                              Error = AuthErrors.InvalidAccessToken
                          };
        var profile = await profileService.GetProfileWithWishesByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

        logger.LogInformation($"GetCurrentProfile response was successfully sent to user with id {userId}");
        return new GetCurrentProfileResponse
        {
            Name = currentUser.Name,
            Surname = currentUser.Surname,
            Patronymic = currentUser.Patronymic ?? "",
            Birthdate = currentUser.BirthDate,
            Profile = mapper.Map<ProfileDto>(profile)
        };
    }
}