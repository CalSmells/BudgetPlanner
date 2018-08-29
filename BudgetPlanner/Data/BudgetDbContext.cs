using System;
using System.Collections.Generic;
using System.Text;
using BudgetPlanner.EntityConfigurations;
using BudgetPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace BudgetPlanner.Data
{
    public class BudgetDbContext : IdentityDbContext, IBudgetDbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {
        }

        const string DbConnection = "Server=(localdb)\\mssqllocaldb;Database=aspnet-BudgetPlanner-77E9E494-078C-4379-B487-F75CABF67F9C;Trusted_Connection=True;MultipleActiveResultSets=true";
        public BudgetDbContext()
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new SubscriptionConfig());
            modelBuilder.ApplyConfiguration(new TransactionConfig());
            modelBuilder.ApplyConfiguration(new CompanyConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());

            modelBuilder.Seed();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
