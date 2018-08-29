using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        //ASYNC
        Task<Company> FirstInclTransSubAsync(Expression<Func<Company, bool>> expression);
        Task<Company> FirstInclTransAsync(Expression<Func<Company, bool>> expression);

        //NONASYNC
        Company FirstInclTransSub(Expression<Func<Company, bool>> expression);
        Company FirstInclTrans(Expression<Func<Company, bool>> expression);
    }
}