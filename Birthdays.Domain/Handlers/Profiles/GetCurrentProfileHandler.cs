using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetCurrentProfileHandler(
    ICurrentUserAccessor userAccessor,
    IProfileService profileService,
    IMapper mapper,
    ILogger<GetCurrentProfileHandler> logger)
{
    public async Task<GetCurrentProfileResponse> Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"GetCurrentProfile request was received from user with id {currentUser.Id}");
        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

        logger.LogInformation("GetCurrentProfile response was successfully sent to user with id " +
                              $"{currentUser.Id}");
        return new GetCurrentProfileResponse
        {
            Name = currentUser.Name,
            Surname = currentUser.Surname,
            Patronymic = currentUser.Patronymic ?? "",
            Birthdate = new DateDto
            {
                Day = currentUser.BirthDate.Day,
                Month = currentUser.BirthDate.Month,
                Year = currentUser.BirthDate.Year
            },
            Profile = mapper.Map<ProfileDto>(profile)
        };
    }
}