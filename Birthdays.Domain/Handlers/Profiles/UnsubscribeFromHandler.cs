using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Validators.Profiles;

namespace Domain.Handlers.Profiles;

public class UnsubscribeFromHandler(
    ICurrentUserAccessor userAccessor,
    UnsubscribeFromRequestValidator validator,
    IProfileService profileService,
    ISubscriptionsService subscriptionsService)
{
    public async Task<UnsubscribeFromResponse> Handle(UnsubscribeFromRequest request, 
        CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);

        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        if (!await profileService.CheckIfProfileExistsAsync(request.BirthdayManId, ct))
        {
            throw new NotFoundException
            {
                Error = ProfilesErrors.NoSuchProfileWithId(request.BirthdayManId)
            };
        }
        
        var unsubscribeResult = await subscriptionsService
            .UnsubscribeAsync(currentUser.ProfileId, request.BirthdayManId, ct);
        if (!unsubscribeResult.IsSuccess)
        {
            throw new NotFoundException
            {
                Error = unsubscribeResult.Error
            };
        }

        return new UnsubscribeFromResponse();
    }
}