using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Responses.Profiles;
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
        logger.LogInformation($"GetCurrentProfileRequest was received " +
                              $"from user with email {currentUser.Email}.");
        
        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        logger.LogInformation($"Profile of user with email {currentUser.Email}" +
                              "was successfully sent to him.");
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