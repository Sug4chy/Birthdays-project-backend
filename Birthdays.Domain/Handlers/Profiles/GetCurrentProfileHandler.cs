using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Validators.Profiles;

namespace Domain.Handlers.Profiles;

public class GetCurrentProfileHandler(
    GetCurrentProfileRequestValidator validator,
    IAuthService authService,
    IProfileService profileService,
    IMapper mapper)
{
    public async Task<GetCurrentProfileResponse> Handle(GetCurrentProfileRequest request,
        CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var currentUser = await authService.GetCurrentUserFromAccessTokenAsync(request.Jwt, ct)
                          ?? throw new UnauthorizedException
                          {
                              Error = AuthErrors.InvalidAccessToken
                          };
        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

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