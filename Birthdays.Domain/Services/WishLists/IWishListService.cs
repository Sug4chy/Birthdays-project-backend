using Data.Entities;
using Domain.DTO;

namespace Domain.Services.WishLists;

public interface IWishListService
{
    Task CreateWishListAsync(WishListDto dto, Profile birthdayMan,
        CancellationToken ct = default);
}