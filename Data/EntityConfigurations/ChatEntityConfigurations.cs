using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class ChatEntityConfigurations : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(c => c.Id);
        builder.HasAlternateKey(c => c.ChatUrl);

        builder.HasOne(c => c.BirthdayMan)
            .WithMany(p => p.Chats)
            .HasForeignKey(c => c.BirthdayManId);

        builder.Property(c => c.ChatType)
            .HasConversion<string>();
    }
}