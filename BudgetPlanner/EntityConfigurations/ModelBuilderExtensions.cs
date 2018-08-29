using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.EntityConfigurations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Company company = new Company
            {
                CompanyName = "Amazon",
                CompanyId = 1,
                Transactions = new List<Transaction>() { },
                Subscriptions = new List<Subscription>() { }
            };
            modelBuilder.Entity<Company>()
                .HasData(company);
        }
    }
}
