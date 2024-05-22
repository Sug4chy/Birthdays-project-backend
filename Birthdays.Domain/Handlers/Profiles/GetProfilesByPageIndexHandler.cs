using AutoMapper;
using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Profiles;

public class GetProfilesByPageIndexHandler(
    GetProfilesByPageIndexRequestValidator validator,
    ICurrentUserAccessor userAccessor,
    IUserService userService,
    IMapper mapper,
    ILogger<GetProfilesByPageIndexHandler> logger)
{
    public async Task<GetProfilesByPageIndexResponse> Handle(GetProfilesByPageIndexRequest request, 
        CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUser.Email}.");

        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var users = await userService
            .GetAllUsersWithPaginationIndexAsync(currentUser.Id, 15 * request.PageIndex, 15, ct);
        logger.LogInformation($"Profiles on page {request.PageIndex} was successfully sent " +
                              $"to user with email {currentUser.Email}.");
        return new GetProfilesByPageIndexResponse
        {
            Profiles = users
                
                .Select(mapper.Map<MainPageProfileDto>).ToArray()
        };
    }
}