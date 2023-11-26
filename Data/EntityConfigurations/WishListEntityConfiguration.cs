using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class WishListEntityConfiguration : IEntityTypeConfiguration<WishList>
{
    public void Configure(EntityTypeBuilder<WishList> builder)
    {
        builder.ToTable("WishLists");

        builder.HasKey(wl => wl.Id);

        builder.HasOne(wl => wl.BirthdayMan)
            .WithMany(p => p.WishLists)
            .HasForeignKey(wl => wl.BirthdayManId);

        builder.HasMany(wl => wl.Wishes)
            .WithOne(w => w.WishList)
            .HasForeignKey(w => w.WishListId);
    }
}