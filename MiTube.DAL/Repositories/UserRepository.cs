using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    //public class UserRepository : IUserRepository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        //public User GetById(Guid id)
        //{
        //    return dbSet
        //        .AsNoTracking()
        //        .FirstOrDefault(t => t.Id == id);
        //}

        //public async Task<User> GetByIdAsync(Guid id)
        //{
        //    //return await dbSet.FindAsync(id);
        //    return await dbSet
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //}

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await Task.Run(() => dbSet
                    .AsNoTracking()
                    .Include(p => p.Playlists)
                    .Include(v => v.Videos)
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions)
                    .Include(uc => uc.Usercredentials)
                    .Include(ut => ut.UserType)
                    .Include(s => s.Subscriptions).ToList());
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(user => user.Id.Equals(id))
                        .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdWithDetailsAsync(Guid id)
        {
            return await FindByCondition(user => user.Id.Equals(id))
                    .Include(p => p.Playlists)
                    .Include(v => v.Videos)
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions)
                    .Include(uc => uc.Usercredentials)
                    .Include(ut => ut.UserType)
                    .Include(s => s.Subscriptions)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(String search)
        {
            return await Task.Run(() => FindByCondition(user => 
                user.Name.Contains(search) 
                || user.Nickname.Contains(search)
                || user.Description.Contains(search)).ToList());
        }

        public async Task<IEnumerable<User>> SearchWithDetailsAsync(String search)
        {
            return await Task.Run(() => FindByCondition(user =>
                user.Name.Contains(search)
                || user.Nickname.Contains(search)
                || user.Description.Contains(search))
                    .Include(p => p.Playlists)
                    .Include(v => v.Videos)
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions)
                    .Include(uc => uc.Usercredentials)
                    .Include(ut => ut.UserType)
                    .Include(s => s.Subscriptions).ToList());
        }



    }
}
