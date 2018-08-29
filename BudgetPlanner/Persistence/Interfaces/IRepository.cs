using BudgetPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.Persistence.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //ASYNC
        //GetCurrentUser
        Task<User> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal User);
        //Get
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        //Add
        void AddAsync(TEntity entity);
        void AddRangeAsync(IEnumerable<TEntity> entities);
         
        //AsNoTracking
        Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync();
        //Where
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression);


        //NONASYNC
        //Get
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        TEntity First(Expression<Func<TEntity, bool>> expression);
        bool Any(Expression<Func<TEntity, bool>> expression);
        //Add
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        //Remove
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        //Update
        void Update(TEntity entity);
        //AsNoTracking
        IEnumerable<TEntity> GetAllAsNoTracking();
        //Where
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    }
}
