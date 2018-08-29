using BudgetPlanner.Data;
using BudgetPlanner.Models;
using BudgetPlanner.Persistence;
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
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BudgetDbContext context, UserManager<User> userManager)
            : base(context, userManager)
        {
        }

        //First
        public async Task<Transaction> FirstInclSubUserAsync(Expression<Func<Transaction, bool>> expression)
        {
            return await BudgetContext.Transaction
                .Include(t => t.Subscription)
                .Include(t => t.User)
                .FirstOrDefaultAsync(expression);
        }
        public async Task<Transaction> FirstInclUserAsync(Expression<Func<Transaction, bool>> expression)
        {
            return await BudgetContext.Transaction
                .Include(t => t.User)
                .FirstOrDefaultAsync(expression);
        }
        //Where
        public async Task<IEnumerable<Transaction>> GetMostExpensiveTransactionsAsync(int count)
        {
            return await BudgetContext.Transaction.OrderByDescending(t => t.Amount).Take(count).ToListAsync();
        }
        public async Task<IEnumerable<Transaction>> WhereDescDateInclCompanyUserAsync(Expression<Func<Transaction, bool>> expression)
        {
            return await BudgetContext.Transaction
                .Include(t => t.Company)
                .Include(t => t.User)
                .Where(expression)
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();
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
