using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace MiTube.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public Task<IEnumerable<T>> GetAllAsync();
        public Task CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        public Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
    }
}
