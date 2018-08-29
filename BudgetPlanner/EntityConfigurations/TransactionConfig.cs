using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetPlanner.Models;

namespace BudgetPlanner.EntityConfigurations
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> modelBuilder)
        {
            modelBuilder
                .HasKey(t => t.TransId);
            modelBuilder
                .Property(t => t.DateTime)
                .HasDefaultValue(DateTime.Now);
            modelBuilder
                .Property(t => t.Amount)
                .IsRequired();
            modelBuilder
                .Property(t => t.TransactionName)
                .IsRequired();
            modelBuilder
                .Property(t => t.UserId)
                .IsRequired();
        }
    }
}
