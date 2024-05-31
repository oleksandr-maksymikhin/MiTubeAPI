using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public Task<IEnumerable<T>> GetAllAsync();

        //???????????? can be Guid or int
        //public void Create(T entity);
        public Task CreateAsync(T entity);
        //public void Update(T entity);
        public Task<T> UpdateAsync(T entity);
        //public void Delete(T entity);
        public Task DeleteAsync(T entity);

        //public IEnumerable<T> FindByCondition(T entity);
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        public Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// ??????????? do we have to realize interface IDisposable
        /// </summary>

        //public void Save();
        //public Task SaveAsync();
    }
}
