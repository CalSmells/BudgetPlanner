using BudgetPlanner.Data;
using BudgetPlanner.Models;
using BudgetPlanner.Persistence;
using BudgetPlanner.Persistence.Interfaces;
using BudgetPlanner.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BudgetDbContext _context;
        private readonly UserManager<User> _userManager;

        public UnitOfWork(BudgetDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            Subscriptions = new SubscriptionRepository(_context, _userManager);
            Transactions = new TransactionRepository(_context, _userManager);
            Companies = new CompanyRepository(_context, _userManager);
            Users = new UserRepository(_context, _userManager);
        }

        public ISubscriptionRepository Subscriptions { get; private set; }
        public ITransactionRepository Transactions { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IUserRepository Users { get; private set; }

        public void AddPayment(Subscription sub, User user, Company company)
        {
            Transaction trans = new Transaction
            {
                Amount = sub.Amount,
                Company = company,
                CompanyId = company.CompanyId,
                DateTime = DateTime.Now,
                Subscription = sub,
                SubId = sub.SubscriptionId,
                User = user,
                UserId = sub.UserId,
                TransactionName = sub.SubscriptionName
            };
            this.Transactions.Add(trans);
            this.Complete();
            sub.Progress += sub.Amount;
            user.Balance += trans.Amount;
            sub.UpdateDates(sub.NextDue);
            sub.Transactions.Add(trans);
            user.Transactions.Add(trans);
            company.Transactions.Add(trans);
            this.Subscriptions.Update(sub);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
