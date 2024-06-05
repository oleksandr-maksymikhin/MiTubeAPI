using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DbContext context) : base(context)
        {
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Comment?> GetByIdWithEverythingAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.Video)
                .FirstOrDefaultAsync();
        }

        public async Task<Comment?> GetByIdWithUserAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Comment?> GetByIdWithVideoAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.Video)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }
    }
}
