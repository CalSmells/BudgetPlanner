using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        //ASYNC
        Task<IEnumerable<Subscription>> GetSubscriptionsWithMostPaymentsAsync(int count);
        Task<IEnumerable<Subscription>> GetSubscriptionsWithTransactionsAsync(int pageIndex, int pageSize);
        Task<IEnumerable<Subscription>> GetMonthlySubscriptionsAsync();
        Task<IEnumerable<Subscription>> GetWeeklySubscriptionsAsync();
        Task<IEnumerable<Subscription>> GetAnnualSubscriptionsAsync();
        Task<Subscription> FirstInclCompanyAsync(Expression<Func<Subscription, bool>> expression);
        //Where
        Task<IEnumerable<Subscription>> WhereDescendingInclCompanyAsync(Expression<Func<Subscription, bool>> expression);
        Task<Subscription> FirstInclTransCompanyAsync(Expression<Func<Subscription, bool>> expression);

        //NONASYNC
        IEnumerable<Subscription> GetSubscriptionsWithMostPayments(int count);
        IEnumerable<Subscription> GetSubscriptionsWithTransactions(int pageIndex, int pageSize);
        IEnumerable<Subscription> GetMonthlySubscriptions();
        IEnumerable<Subscription> GetWeeklySubscriptions();
        IEnumerable<Subscription> GetAnnualSubscriptions();
        Subscription FirstInclCompany(Expression<Func<Subscription, bool>> expression);
        //Where
        IEnumerable<Subscription> WhereDescendingInclCompany(Expression<Func<Subscription, bool>> expression);
        Subscription FirstInclTransCompany(Expression<Func<Subscription, bool>> expression);
    }
}
