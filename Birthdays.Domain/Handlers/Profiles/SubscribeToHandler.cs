using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Validators.Profiles;

namespace Domain.Handlers.Profiles;

public class SubscribeToHandler(
    ICurrentUserAccessor userAccessor,
    SubscribeToRequestValidator validator,
    IProfileService profileService,
    ISubscriptionsService subscriptionsService)
{
    public async Task<SubscribeToResponse> Handle(SubscribeToRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        if (!await profileService.CheckIfProfileExistsAsync(request.BirthdayManId, ct))
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