using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetProfileByUsernameHandler(
    GetProfileByUsernameRequestValidator validator,
    IUserService userService,
    IProfileService profileService,
    IAuthService authService,
    IMapper mapper,
    ISubscriptionsService subscriptionsService,
    ILogger<GetProfileByUsernameHandler> logger)
{
    public async Task<GetProfileByUsernameResponse> Handle(GetProfileByUsernameRequest request,
        CancellationToken ct = default)
    {
        logger.LogInformation($"GetProfileByUsername was received for {request.Username}'s profile");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);
        
        var currentUser = await authService.GetCurrentUserFromAccessTokenAsync(request.Jwt, ct)
            ?? throw new UnauthorizedException
            {
                Error = AuthErrors.InvalidAccessToken
            };

        var user = await userService.GetUserByEmailAsync(request.Username, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithEmail(request.Username));

        var profile = await profileService.GetProfileByIdAsync(user!.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(user.ProfileId));

        logger.LogInformation($"GetProfileByUsernameResponse was successfully sent to {currentUser.Email}");
        return new GetProfileByUsernameResponse
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