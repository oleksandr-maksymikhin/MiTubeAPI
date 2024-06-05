using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class UsercredentialsRepository : GenericRepository<Usercredentials>, IUsercredentialsRepository
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
    }
}
