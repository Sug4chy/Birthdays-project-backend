using Data.Entities;

namespace Domain.Services.WishLists;

public interface IWishListService
{
    Task CreateWishListAsync(Profile birthdayMan, string name, string? description,
        CancellationToken ct = default);
}