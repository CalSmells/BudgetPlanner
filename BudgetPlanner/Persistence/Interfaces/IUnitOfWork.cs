using System;
using BudgetPlanner.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetPlanner.Models;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISubscriptionRepository Subscriptions { get; }
        ITransactionRepository Transactions { get; }
        ICompanyRepository Companies { get; }
        IUserRepository Users { get; }
        void AddPayment(Subscription sub, User user, Company company);
        int Complete();
        Task<int> CompleteAsync();
    }
}
