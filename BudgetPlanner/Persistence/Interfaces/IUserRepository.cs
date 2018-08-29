using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //ASYNC
        Task<User> FirstInclTransactionAsync(Expression<Func<User, bool>> expression);

        //NONASYNC
        User FirstInclTransaction(Expression<Func<User, bool>> expression);
    }
}
