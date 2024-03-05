using Data.Entities;
using Domain.DTO;

namespace Domain.Services.WishLists;

public interface IWishListService
{
    Task CreateWishListAsync(WishListDto dto, Profile birthdayMan, CancellationToken ct = default);
    Task<List<WishList>> GetWishListsByProfileIdAsync(Guid profileId, CancellationToken ct = default);
    Task<WishList?> GetWishListByIdAsync(Guid wishListId, CancellationToken ct = default);
    Task CreateWishAsync(WishDto wishDto, WishList wishList, CancellationToken ct = default);
    Task UpdateWishListAsync(WishList wishList, string name, string? description, CancellationToken ct = default);
    Task DeleteWishListAsync(WishList wishList, CancellationToken ct = default);
}