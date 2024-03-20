using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Validators.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class SubscribeToHandler(
    ICurrentUserAccessor userAccessor,
    SubscribeToRequestValidator validator,
    IProfileService profileService,
    ISubscriptionsService subscriptionsService,
    ILogger<SubscribeToHandler> logger)
{
    public async Task Handle(SubscribeToRequest request, CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"{request.GetType().Name} for user with id {request.BirthdayManId} " +
                              $"was received from user with email {currentUser.Email}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);
        
        if (!await profileService.CheckIfProfileExistsAsync(request.BirthdayManId, ct))
        {
            throw new NotFoundException
            {
                Error = ProfilesErrors.NoSuchProfileWithId(request.BirthdayManId)
            };
        }

        await subscriptionsService.SubscribeAsync(currentUser.ProfileId, request.BirthdayManId, ct);
        logger.LogInformation($"User with email {currentUser.Email} was successfully subscribed " +
                              $"to user with id {request.BirthdayManId}.");
    }
}