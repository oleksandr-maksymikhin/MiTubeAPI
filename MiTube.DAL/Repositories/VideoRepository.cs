using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class VideoRepository: GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Video>> GetAllWithDetailsAsync()
        {
            return await Task.Run(() => GetAll()
                .Include(c => c.Comments)
                .Include(i => i.Interactions).ToList());
        }

        public async Task<Video?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                        .FirstOrDefaultAsync();
        }

        public async Task<Video?> GetByIdWithDetailsAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Video>?> GetByUserIdWithDetailsAsync(Guid userId)
        {
            return await Task.Run(() => FindByCondition(video => video.UserId.Equals(userId))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions).ToList());
        }

        public async Task<IEnumerable<Video>> SearchAsync(String search)
        {
            return await Task.Run(() => FindByCondition(video => video.Description
                .Contains(search) || video.Title.Contains(search)).ToList());
        }

        public async Task<IEnumerable<Video>> SearchWithDetailsAsync(String search)
        {
            return await Task.Run(() => FindByCondition(video => video.Description
                .Contains(search) || video.Title.Contains(search))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions).ToList());
        }
    }
}
