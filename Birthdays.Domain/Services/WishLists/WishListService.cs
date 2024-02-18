using Data.Context;
using Data.Entities;
using Domain.DTO;

namespace Domain.Services.WishLists;

public class WishListService(AppDbContext context) : IWishListService
{
    public async Task CreateWishListAsync(WishListDto dto, Profile birthdayMan, CancellationToken ct = default)
    {
        var wishList = new WishList
        {
            BirthdayMan = birthdayMan,
            BirthdayManId = birthdayMan.Id,
            Description = dto.Description,
            Name = dto.Name
        };
        await context.WishLists.AddAsync(wishList, ct);
        await context.Wishes.AddRangeAsync(dto.Wishes
                .Select(wishDto => new Wish
                {
                    Name = wishDto.Name,
                    WishListId = wishList.Id,
                    WishList = wishList,
                    Description = wishDto.Description,
                    GiftRef = wishDto.GiftRef,
                }), ct);

        await context.SaveChangesAsync(ct);
    }
}