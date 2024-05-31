using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<IEnumerable<User>> GetAllWithDetailsAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByIdWithDetailsAsync(Guid id);

        public Task<IEnumerable<User>> SearchAsync(String search);
        public Task<IEnumerable<User>> SearchWithDetailsAsync(String search);
    }
}
