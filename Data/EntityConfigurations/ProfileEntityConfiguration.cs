using Birthdays.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birthdays.Data.EntityConfigurations;

public class ProfileEntityConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<User>(u => u.ProfileId);

        builder.HasMany(p => p.Chats)
            .WithOne(c => c.BirthdayMan)
            .HasForeignKey(c => c.BirthdayManId);

        builder.HasMany(p => p.Subscriptions)
            .WithOne(s => s.BirthdayMan)
            .HasForeignKey(s => s.BirthdayManId);

        builder.HasMany(p => p.WishLists)
            .WithOne(wl => wl.BirthdayMan)
            .HasForeignKey(wl => wl.BirthdayManId);
    }
}