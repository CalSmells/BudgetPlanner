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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(BudgetDbContext context, UserManager<User> userManager)
            : base(context, userManager)
        {
        }
        //ASYNC
        public async Task<Company> FirstInclTransSubAsync(Expression<Func<Company, bool>> expression)
        {
            return await BudgetContext.Company
                .Include(c => c.Transactions)
                .Include(c => c.Subscriptions)
                .FirstOrDefaultAsync(expression);
        }
        public async Task<Company> FirstInclTransAsync(Expression<Func<Company, bool>> expression)
        {
            return await BudgetContext.Company
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(expression);
        }

        //NONASYNC
        public Company FirstInclTransSub(Expression<Func<Company, bool>> expression)
        {
            return BudgetContext.Company
                .Include(c => c.Transactions)
                .Include(c => c.Subscriptions)
                .FirstOrDefault(expression);
        }
        public Company FirstInclTrans(Expression<Func<Company, bool>> expression)
        {
            return BudgetContext.Company
                .Include(c => c.Transactions)
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
