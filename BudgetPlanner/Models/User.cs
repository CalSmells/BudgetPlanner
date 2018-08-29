using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class User : IdentityUser
    {
        public decimal Balance { get; set; } // public set - insecure
        
        public List<Transaction> Transactions { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
