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

    public Task<WishList?> GetWishListByIdAsync(Guid wishListId, CancellationToken ct = default)
        => context.WishLists
            .Include(wl => wl.Wishes)
            .FirstOrDefaultAsync(wl => wl.Id == wishListId, ct);

    public async Task CreateWishAsync(WishDto wishDto, WishList wishList, CancellationToken ct = default)
    {
        var wish = new Wish
        {
            Description = wishDto.Description,
            GiftRef = wishDto.GiftRef,
            Name = wishDto.Name,
            WishList = wishList,
            WishListId = wishList.Id
        };
        wishList.Wishes!.Add(wish);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateWishListAsync(WishList wishList, string name, string? description,
        CancellationToken ct = default)
    {
        wishList.Name = name;
        wishList.Description = description;
        context.WishLists.Update(wishList);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateWishAsync(Wish wish, WishDto dto, CancellationToken ct = default)
    {
        wish.Name = dto.Name;
        wish.Description = dto.Description;
        wish.GiftRef = dto.GiftRef;
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteWishListAsync(WishList wishList, CancellationToken ct = default)
    {
        context.WishLists.Remove(wishList);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteWishAsync(Wish wish, CancellationToken ct = default)
    {
        var wishList = wish.WishList;
        wishList!.Wishes!.Remove(wish);
        await context.SaveChangesAsync(ct);
    }
}