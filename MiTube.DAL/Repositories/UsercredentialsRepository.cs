using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class UsercredentialsRepository : GenericRepository<Usercredentials>, IUsercredentialsRepository
    //public class UserRepository : IUserRepository<User>
    {
        public UsercredentialsRepository(DbContext context) : base(context)
        {
        }

        public async Task<Usercredentials?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(user => user.Id.Equals(id))
                        .FirstOrDefaultAsync();
        }

        public async Task<Usercredentials?> SearchByEmailAsync(String email)
        {
            return await Task.Run(() => FindByCondition(usercredentials =>
                    usercredentials.Email.Contains(email))
                        .FirstOrDefaultAsync());

        }

        public async Task<Usercredentials?> SearchEntityAsync(Usercredentials usercredentials)
        {
            return await Task.Run(() => FindByCondition(uc =>
                uc.Email.Equals(usercredentials.Email)
                && uc.Password.Equals(usercredentials.Password))
                    .FirstOrDefaultAsync());
        }

        //public Usercredentials GetById(Guid id)
        //{
        //    return dbSet
        //        .AsNoTracking()
        //        .FirstOrDefault(t => t.Id == id);
        //}

        //public async Task<Usercredentials> GetByIdAsync(Guid id)
        //{
        //    //return await dbSet.FindAsync(id);
        //    return await dbSet
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //}


    }
}
