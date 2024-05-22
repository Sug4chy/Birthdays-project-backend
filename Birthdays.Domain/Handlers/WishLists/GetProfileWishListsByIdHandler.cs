using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class GetProfileWishListsByIdHandler(
    ICurrentUserAccessor userAccessor,
    GetProfileWishListsByIdRequestValidator validator,
    IUserService userService,
    IWishListService wishListService,
    IMapper mapper,
    ILogger<GetProfileWishListsByIdHandler> logger)
{
    public async Task<GetProfileWishListsResponse> Handle(GetProfileWishListsByIdRequest request, 
        CancellationToken ct = default)
    {
        string currentUserEmail = await userAccessor.GetCurrentUserEmailAsync(ct);
        logger.LogInformation($"{request.GetType().Name} for user with id {request.UserId} " +
                              $"was received from user with email {currentUserEmail}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var user = await userService.GetUserByIdAsync(request.UserId, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithId(request.UserId));
        var wishLists = await wishListService.GetWishListsByProfileIdAsync(user!.ProfileId, ct);

        logger.LogInformation($"WishLists of user with id {request.UserId} were successfully " +
                              $"sent to user with email {currentUserEmail}.");
        return new GetProfileWishListsResponse
        {
            WishLists = wishLists.Select(mapper.Map<WishListDto>).ToArray()
        };
    }
}