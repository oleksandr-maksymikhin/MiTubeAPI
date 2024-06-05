using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class InteractionRepository : GenericRepository<Interaction>, IInteractionRepository
    {
        public InteractionRepository(DbContext context) : base(context)
        { 
        }

        public async Task<Interaction?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                       .FirstOrDefaultAsync();
        }

        public async Task<Interaction?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId)
        {
            return await FindByCondition(interaction => interaction.UserId.Equals(userId) && interaction.VideoId.Equals(videoId))
                       .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Interaction>?> GetByUserIdAsync(Guid userId)
        {
            return await Task.Run(() => FindByCondition(interaction => interaction.UserId.Equals(userId)).ToList());
        }
    }
}