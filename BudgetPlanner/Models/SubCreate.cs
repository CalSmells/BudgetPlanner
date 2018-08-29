using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class SubCreate
    {
        public int SubId { get; set; }
        [Display(Name = "Name")]
        [Required, StringLength(15)]
        public string SubscriptionName { get; set; } = "";
        public string Interval { get; set; } = "month";
        public Company Company { get; set; }
        [Required, Display(Name = "Amount (£)")]
        public decimal Amount { get; set; }
        public decimal? Target { get; set; } = null;
        [Display(Name = "First Payment")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime NextDate { get; set; } = DateTime.Now.AddMonths(1);
        public bool InOut { get; set; }
        
        public SelectList Companies { get; set; }
        public int CompanyId { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
