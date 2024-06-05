using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Interfaces;
using System.Linq.Expressions;

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

        public IQueryable<T> GetAll() => dbSet.AsNoTracking();

        public async Task<IEnumerable<T>> GetAllAsync() => await Task.Run(() => dbSet.AsNoTracking().ToList());

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            int resultAsync = await context.SaveChangesAsync();
        }
        
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

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).AsNoTracking();
        }

        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => FindByCondition(expression));
        }
        public void Save() => context.SaveChanges();
        public async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
