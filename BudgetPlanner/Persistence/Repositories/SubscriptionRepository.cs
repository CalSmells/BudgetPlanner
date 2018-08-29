using BudgetPlanner.Data;
using BudgetPlanner.Models;
using BudgetPlanner.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(BudgetDbContext context, UserManager<User> userManager)
            : base(context, userManager)
        {
        }
        //ASYNC
        public async Task<IEnumerable<Subscription>> GetSubscriptionsWithMostPaymentsAsync(int count)
        {
            return await BudgetContext.Subscription
                .OrderByDescending(s => s.Transactions.Count)
                .Take(count)
                .ToListAsync();
        }
        public async Task<IEnumerable<Subscription>> GetSubscriptionsWithTransactionsAsync(int pageIndex, int pageSize)
        {
            return await BudgetContext.Subscription
                .Include(s => s.Transactions)
                .OrderBy(s => s.SubscriptionName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Subscription>> GetMonthlySubscriptionsAsync()
        {
            return await BudgetContext.Subscription
                .Where(s => s.Interval == "0,1,0")
                .ToListAsync();
        }
        public async Task<IEnumerable<Subscription>> GetWeeklySubscriptionsAsync()
        {
            return await BudgetContext.Subscription
                .Where(s => s.Interval == "7,0,0")
                .ToListAsync();
        }
        public async Task<IEnumerable<Subscription>> GetAnnualSubscriptionsAsync()
        {
            return await BudgetContext.Subscription
                .Where(s => s.Interval == "0,0,1")
                .ToListAsync();
        }
        public async Task<Subscription> FirstInclCompanyAsync(Expression<Func<Subscription, bool>> expression)
        {
            return await BudgetContext.Subscription
                .Include(s => s.Company)
                .FirstOrDefaultAsync(expression);
        }
        public async Task<IEnumerable<Subscription>> WhereDescendingInclCompanyAsync(Expression<Func<Subscription, bool>> expression)
        {
            return await BudgetContext.Subscription
                    .Include(s => s.Company)
                    .Where(expression)
                    .OrderByDescending(s => s.NextDue)
                    .ToListAsync();
        }
        public async Task<Subscription> FirstInclTransCompanyAsync(Expression<Func<Subscription, bool>> expression)
        {
            return await BudgetContext.Subscription
                .Include(s => s.Transactions)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(expression);
        }

        //NONASYNC
        public IEnumerable<Subscription> GetSubscriptionsWithMostPayments(int count)
        {
            return BudgetContext.Subscription.OrderByDescending(s => s.Transactions.Count).Take(count).ToList();
        }
        public IEnumerable<Subscription> GetSubscriptionsWithTransactions(int pageIndex, int pageSize)
        {
            return BudgetContext.Subscription
                .Include(s => s.Transactions)
                .OrderBy(s => s.SubscriptionName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public IEnumerable<Subscription> GetMonthlySubscriptions()
        {
            return BudgetContext.Subscription
                .Where(s => s.Interval == "0,1,0")
                .ToList();
        }
        public IEnumerable<Subscription> GetWeeklySubscriptions()
        {
            return BudgetContext.Subscription
                .Where(s => s.Interval == "7,0,0")
                .ToList();
        }
        public IEnumerable<Subscription> GetAnnualSubscriptions()
        {
            return BudgetContext.Subscription
                .Where(s => s.Interval == "0,0,1")
                .ToList();
        }
        public Subscription FirstInclCompany(Expression<Func<Subscription, bool>> expression)
        {
            return BudgetContext.Subscription
                .Include(s => s.Company)
                .FirstOrDefault(expression);
        }
        public IEnumerable<Subscription> WhereDescendingInclCompany(Expression<Func<Subscription, bool>> expression)
        {
            return BudgetContext.Subscription
                    .Include(s => s.Company)
                    .Where(expression)
                    .OrderByDescending(s => s.NextDue).ToList();
        }
        public Subscription FirstInclTransCompany(Expression<Func<Subscription, bool>> expression)
        {
            return BudgetContext.Subscription
                .Include(s => s.Transactions)
                .Include(s => s.Company)
                .FirstOrDefault(expression);
        }

        public UserManager<User> _userManager
        {
            get { return UserManager as UserManager<User>; }
        }

        public BudgetDbContext BudgetContext
        {
            get { return Context as BudgetDbContext; }
        }
    }
}
    