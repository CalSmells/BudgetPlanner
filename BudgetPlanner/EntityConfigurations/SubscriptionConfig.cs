using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.EntityConfigurations
{
    public class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> modelBuilder)
        {
            modelBuilder
                .HasMany(s => s.Transactions)
                .WithOne(t => t.Subscription)
                .HasForeignKey(t => t.SubId);
            modelBuilder
                .HasOne(s => s.Company)
                .WithMany(c => c.Subscriptions);
            modelBuilder
                .Property(s => s.SubscriptionName)
                .IsRequired()
                .HasMaxLength(25);
            modelBuilder
                .Property(s => s.Amount)
                .IsRequired()
                .HasDefaultValue((decimal)10.00);
            modelBuilder
                .Property(s => s.Interval)
                .IsRequired()
                .HasDefaultValue("0,1,0");
            modelBuilder
                .Property(s => s.OverDue)
                .IsRequired()
                .HasDefaultValue("More");
            modelBuilder
                .Property(s => s.NextDue)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
