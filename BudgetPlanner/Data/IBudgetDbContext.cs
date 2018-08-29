using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Data
{
    public interface IBudgetDbContext 
    {
        DbSet<User> User { get; set; }
        DbSet<Subscription> Subscription { get; set; }
        DbSet<Company> Company { get; set; }
        DbSet<Transaction> Transaction { get; set; }
    }
}
