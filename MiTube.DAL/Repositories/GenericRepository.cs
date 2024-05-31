using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class GenericRepository<T> : IRepository <T> where T : class
    {
        private readonly DbContext context;
        protected DbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        //// ????? IQueryable or IEnumerable ??????
        public IQueryable<T> GetAll() => dbSet.AsNoTracking();
        public async Task<IEnumerable<T>> GetAllAsync() => await Task.Run(() => dbSet.AsNoTracking().ToList());

        //public void Create(T entity)
        //{
        //    dbSet.Add(entity);
        //    context.SaveChanges();
        //}
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            int resultAsync = await context.SaveChangesAsync();
        }

        //public void Update(T entity)
        //{
        //    context.Entry(entity).State = EntityState.Modified;

        //    ////alternative option:
        //    //dbSet.Update(entity);
        //    context.SaveChanges();
        //}
        
        public async Task<T> UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return entity;
        }

        //public void Delete(T entity)
        //{
        //    dbSet.Remove(entity);
        //    context.SaveChanges();
        //}
        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);                           //error -> consider for a solution (option -> like in BoxOffice project)
            await context.SaveChangesAsync();
        }

        //find by condition synchronously
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).AsNoTracking();
        }

        //find by condition asynchronously - not in use
        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => FindByCondition(expression));
        }
        public void Save() => context.SaveChanges();
        //public async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
