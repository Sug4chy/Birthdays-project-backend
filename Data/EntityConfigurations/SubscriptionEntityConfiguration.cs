using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.BirthdayMan)
            .WithMany(p => p.SubscriptionsAsBirthdayMan)
            .HasForeignKey(s => s.BirthdayManId);

        builder.HasOne(s => s.Subscriber)
            .WithMany(p => p.SubscriptionsAsSubscriber)
            .HasForeignKey(s => s.SubscriberId);
    }
}