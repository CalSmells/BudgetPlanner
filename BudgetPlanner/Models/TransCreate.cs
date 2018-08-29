using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class TransCreate
    {
        public SelectList CompanyIds { get; set; }
        public bool InOut { get; set; }
        public int CompanyId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Display(Name = "Amount (£)")]
        public int Amount { get; set; }
        public int SubId { get; set; }
        [Display(Name ="Transaction Name")]
        public string TransName { get; set; }
    }
}
