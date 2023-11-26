using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.HasAlternateKey(u => u.Email);

        builder.HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<User>(u => u.ProfileId);
    }
}