using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.EntityConfigurations
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> modelBuilder)
        {
            modelBuilder
                .HasKey(c => c.CompanyId);
            modelBuilder
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId);
            modelBuilder
                .HasMany(c => c.Subscriptions)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId);

            modelBuilder
                .Property(c => c.CompanyName)
                .HasDefaultValue("CompanyName")
                .HasMaxLength(25)
                .IsRequired();
        }
    }
}
