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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly UserManager<User> UserManager;

        public Repository(DbContext context, UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }
        //ASYNC
        //GetCurrentUser
        public async Task<User> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal User)
        {
            return await UserManager.GetUserAsync(User);
        }
        //Get
        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().Where(expression).AnyAsync();
        }
        //Add
        public async void AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }
        public async void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }
        //AsNoTracking
        public async Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync()
        {
            return await Context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }
        //Where
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>()
                .Where(expression)
                .ToListAsync();
        }

        //NONASYNC
        //Get
        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }
        public TEntity First(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Where(expression).Any();
        }
        //Add
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }
        //Update
        public void Update(TEntity entity)
        {
            Context.Update<TEntity>(entity);
        }
        //Remove
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        //AsNoTracking
        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return Context.Set<TEntity>()
                .AsNoTracking()
                .ToList();
        }
        //Where
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>()
                .Where(expression)
                .ToList();
        }
    }
}
