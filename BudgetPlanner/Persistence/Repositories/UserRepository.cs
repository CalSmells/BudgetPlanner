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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BudgetDbContext context, UserManager<User> userManager)
            : base(context, userManager)
        {
        }
        //ASYNC
        public async Task<User> FirstInclTransactionAsync(Expression<Func<User, bool>> expression)
        {
            return await BudgetContext.User
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(expression);
        }

        //NONASYNC
        public User FirstInclTransaction(Expression<Func<User, bool>> expression)
        {
            return BudgetContext.User
                .Include(u => u.Transactions)
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
