using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class WishEntityConfiguration : IEntityTypeConfiguration<Wish>
{
    public void Configure(EntityTypeBuilder<Wish> builder)
    {
        builder.ToTable("Wishes");

        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.Image)
            .WithOne(f => f.Wish)
            .HasForeignKey<Wish>(w => w.ImageId);

        builder.HasOne(w => w.WishList)
            .WithMany(wl => wl.Wishes)
            .HasForeignKey(w => w.WishListId);
    }
}