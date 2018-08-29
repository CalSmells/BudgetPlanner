using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class Transaction
    {
        public int TransId { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; } = 0;
        public string TransactionName { get; set; }


        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Subscription Subscription { get; set; } = null;
        public int? SubId { get; set; } = null;
    }
}
