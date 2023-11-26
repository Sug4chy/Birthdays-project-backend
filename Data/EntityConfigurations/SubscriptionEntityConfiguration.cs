using Birthdays.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Birthdays.Data.EntityConfigurations;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.BirthdayMan)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.BirthdayManId);

        builder.HasOne(s => s.Subscriber)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.SubscriberId);
    }
}