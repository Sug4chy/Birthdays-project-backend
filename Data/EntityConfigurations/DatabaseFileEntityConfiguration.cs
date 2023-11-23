using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class DatabaseFileEntityConfiguration : IEntityTypeConfiguration<DatabaseFile>
{
    public void Configure(EntityTypeBuilder<DatabaseFile> builder)
    {
        builder.ToTable("Images");

        builder.HasKey(f => f.Id);
        builder.HasAlternateKey(f => f.Name);
        builder.HasAlternateKey(f => f.FileRef);

        builder.HasOne(f => f.Wish)
            .WithOne(w => w.Image);
    }
}