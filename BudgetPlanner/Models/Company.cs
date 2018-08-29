    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public List<Transaction> Transactions { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
