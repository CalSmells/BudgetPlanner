using BudgetPlanner.Data;
using BudgetPlanner.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner
{
    public class BudgetHub : Hub
    {
        private readonly BudgetDbContext _context;

        public BudgetHub(BudgetDbContext context)
        {
            _context = context;
        }

        public async void Ping()
        {
            await Clients.Caller.SendAsync("pong");
        }

        public async void UpdateSubs(string userId = "")
        {
            List<Subscription> subs = _context.Subscription.Where(s => s.UserId == userId).ToList();
            List<string> subNames = new List<string>() { };
            List<int> due = new List<int>() { };
            var date = DateTime.Now.Date;

            for (var i=0; i < subs.Count; i++)
            {
                if (subs[i].NextDue.Date <= date)
                {
                    due.Add(subs[i].SubscriptionId);
                    subs[i].OverDue = "OVERDUE";
                    subNames.Add(subs[i].SubscriptionName);
                }
                else
                {
                    subs[i].OverDue = "More";
                }
            }
            await _context.SaveChangesAsync();
            await Clients.Caller.SendAsync("UpdateSubs", due);
            await Clients.Caller.SendAsync("subDue", subNames);
        }
    }
}
