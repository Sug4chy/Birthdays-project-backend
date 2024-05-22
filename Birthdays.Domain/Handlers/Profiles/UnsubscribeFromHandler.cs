using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Validators.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class UnsubscribeFromHandler(
    ICurrentUserAccessor userAccessor,
    UnsubscribeFromRequestValidator validator,
    IProfileService profileService,
    ISubscriptionsService subscriptionsService,
    ILogger<UnsubscribeFromHandler> logger)
{
    public async Task Handle(UnsubscribeFromRequest request, 
        CancellationToken ct = default)
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
        
        var unsubscribeResult = await subscriptionsService
            .UnsubscribeAsync(currentUser.ProfileId, request.BirthdayManId, ct);
        logger.LogInformation($"User with email {currentUser.Email} was successfully unsubscribed " +
                              $"from user with id {request.BirthdayManId}.");
        if (!unsubscribeResult.IsSuccess)
        {
            throw new NotFoundException
            {
                Error = unsubscribeResult.Error
            };
        }
    }
}