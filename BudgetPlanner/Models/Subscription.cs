using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Models
{
    public class Subscription
    {
        //Properties
        public int SubscriptionId { get; set; }
        [Display(Name = "Name")]
        public string SubscriptionName { get; set; } = "";
        public Company Company { get; set; } //"inherited" by new transactions
        public int CompanyId { get; set; }
        public decimal Amount { get; set; }//"inherited" by new transactions
        public decimal Progress { get; set; }
        [Range(0.10, double.MaxValue)]
        public decimal? Target { get; set; } = null;
        public bool InOut { get; set; }
        public string Interval { get; set; } = "0,1,0";
        public string OverDue { get; set; } = "More";


        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        [Display(Name = "Next Payment")]
        public DateTime NextDue { get; set; }

        public List<Transaction> Transactions { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }


        //Methods

        public void UpdateDates(DateTime Date)
        {
            int[] interval = ParseInterval();
            this.NextDue = AddDifference(interval, 1, Date);
        }

        public int[] ParseInterval()
        {
            string[] intervalStr = this.Interval.Split(",");
            int a = intervalStr.Count();
            int[] interval = new int[3];
            for (int i = 0; i < a; i++)
            {
                int temp = Int32.Parse(intervalStr[i]);
                interval[i] = temp;
            }
            return interval;
        }
        

        private decimal GetPaymentsDecimal()
        {
            decimal payments = (decimal)(this.Target - this.Progress) / this.Amount;
            payments = Math.Round(payments, MidpointRounding.AwayFromZero);
            return payments;
        }

        private DateTime AddDifference(int[] interval, decimal payments, DateTime Date)
        {
            int years = interval[2] * (int)payments;
            int months = interval[1] * (int)payments;
            int days = interval[0] * (int)payments;

            Date = Date.AddYears(years);
            Date = Date.AddMonths(months);
            Date = Date.AddDays(days);
            return Date;
        }

    }
}
