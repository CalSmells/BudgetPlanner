using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BudgetPlanner.Models;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        //First
        Task<Transaction> FirstInclSubUserAsync(Expression<Func<Transaction, bool>> expression);
        Task<Transaction> FirstInclUserAsync(Expression<Func<Transaction, bool>> expression);
        //Where
        Task<IEnumerable<Transaction>> GetMostExpensiveTransactionsAsync(int count);
        Task<IEnumerable<Transaction>> WhereDescDateInclCompanyUserAsync(Expression<Func<Transaction, bool>> expression);
    }
}