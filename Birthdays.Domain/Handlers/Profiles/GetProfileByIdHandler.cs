﻿using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetProfileByIdHandler(
    GetProfileByIdRequestValidator validator,
    IUserService userService,
    IProfileService profileService,
    IMapper mapper,
    ISubscriptionsService subscriptionsService,
    ICurrentUserAccessor userAccessor,
    ILogger<GetProfileByIdHandler> logger)
{

    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request,
        CancellationToken ct = default)
    {
        logger.LogInformation($"GetProfileById request was received for {request.UserId}'s profile");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        
        var user = await userService.GetUserByIdAsync(request.UserId.ToString(), ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithId(request.UserId.ToString()));
        var profile = await profileService.GetProfileByIdAsync(user!.ProfileId, ct);
        
        logger.LogInformation($"GetProfileByUsernameResponse was successfully sent to {currentUser.Email}");
        return new GetProfileByIdResponse
        {
            Name = user.Name,
            Surname = user.Surname,
            Patronymic = user.Patronymic ?? "",
            Birthdate = new DateDto
            {
                Day = currentUser.BirthDate.Day,
                Month = currentUser.BirthDate.Month,
                Year = currentUser.BirthDate.Year
            },
            Profile = mapper.Map<ProfileDto>(profile),
            IsCurrentUserSubscribedTo = await subscriptionsService
                .IsSubscribedToAsync(currentUser.ProfileId, profile!.Id, ct)
        };
    }
}