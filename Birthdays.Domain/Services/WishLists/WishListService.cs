using Data.Context;
using Data.Entities;
using Domain.Results;

namespace Domain.Services.WishLists;

public class WishListService(AppDbContext context) : IWishListService
{
    public async Task CreateWishListAsync(Profile birthdayMan, string name, string? description, 
        CancellationToken ct = default)
    {
        var wishList = new WishList
        {
            BirthdayMan = birthdayMan,
            BirthdayManId = birthdayMan.Id,
            Description = description,
            Name = name
        };
        await context.WishLists.AddAsync(wishList, ct);
    }
}