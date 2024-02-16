using System.Security.Claims;
using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class CreateWishListHandler(
    IUserService userService,
    CreateWishListRequestValidator validator,
    IProfileService profileService,
    IWishListService wishListService,
    IHttpContextAccessor accessor,
    ILogger<CreateWishListHandler> logger)
{
    private readonly HttpContext _context = accessor.HttpContext!;

    public async Task<CreateWishListResponse> Handle(CreateWishListRequest request,
        CancellationToken ct = default)
    {
        string userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? throw new UnauthorizedException
                        {
                            Error = AuthErrors.DoesNotIncludeClaim(ClaimTypes.NameIdentifier)
                        };
        var currentUser = await userService.GetUserByIdAsync(userId, ct)
                          ?? throw new UnauthorizedException
                          {
                              Error = UsersErrors.NoSuchUserWithId(userId)
                          };
        logger.LogInformation($"CreateWishList request was received from {currentUser.Email} user");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

        await wishListService.CreateWishListAsync(profile!, request.Name, request.Description, ct);
        logger.LogInformation($"CreateWishList response was successfully sent to {currentUser.Email} user");
        return new CreateWishListResponse();
    }
}