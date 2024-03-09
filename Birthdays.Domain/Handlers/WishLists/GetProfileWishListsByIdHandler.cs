using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class GetProfileWishListsByIdHandler(
    GetProfileWishListsByIdRequestValidator validator,
    IUserService userService,
    IWishListService wishListService,
    IMapper mapper)
{
    public async Task<GetProfileWishListsResponse> Handle(GetProfileWishListsByIdRequest request, 
        CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var user = await userService.GetUserByIdAsync(request.UserId, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithId(request.UserId));
        var wishLists = await wishListService.GetWishListsByProfileIdAsync(user!.ProfileId, ct);

        return new GetProfileWishListsResponse
        {
            WishLists = wishLists.Select(mapper.Map<WishListDto>).ToArray()
        };
    }
}