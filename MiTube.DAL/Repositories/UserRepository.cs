using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

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
