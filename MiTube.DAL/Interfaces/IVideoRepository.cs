using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        public Task<IEnumerable<Video>> GetAllWithDetailsAsync();
        public Task<Video?> GetByIdAsync(Guid id);
        public Task<Video?> GetByIdWithDetailsAsync(Guid id);
        public Task<IEnumerable<Video>?> GetByUserIdWithDetailsAsync(Guid userId);
        public Task<IEnumerable<Video>> SearchAsync(String search);
        public Task<IEnumerable<Video>> SearchWithDetailsAsync(String search);
    }
}
