using Data.Context;
using Data.Entities;
using Domain.DTO;
using Microsoft.EntityFrameworkCore;

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
            Name = dto.Name,
            Wishes = new List<Wish>()
        };
        foreach (var wishDto in dto.Wishes)
        {
            wishList.Wishes.Add(new Wish
            {
                Name = wishDto.Name,
                WishListId = wishList.Id,
                WishList = wishList,
                Description = wishDto.Description,
                GiftRef = wishDto.GiftRef,
            });
        }

        await context.WishLists.AddAsync(wishList, ct);
        await context.SaveChangesAsync(ct);
    }

    public Task<List<WishList>> GetWishListsByProfileIdAsync(Guid profileId,
        CancellationToken ct = default)
        => context.WishLists
            .Where(wl => wl.BirthdayManId == profileId)
            .Include(wl => wl.Wishes)
            .ToListAsync(ct);
}