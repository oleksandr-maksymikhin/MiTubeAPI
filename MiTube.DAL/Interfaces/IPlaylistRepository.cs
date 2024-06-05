using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        public Task<IEnumerable<Playlist>> GetAllWithDetailsAsync();
        public Task<Playlist?> GetByIdAsync(Guid id);
        public Task<Playlist?> GetByIdWithVideosAsync(Guid id);
        public Task<Playlist?> GetByIdWithUserAsync(Guid id);
        public Task<Playlist?> GetByIdWithEverythingAsync(Guid id);
        public void AddVideo(Guid id, Video video);
        public Task<IEnumerable<Playlist>> GetByUserIdWithDetailsAsync(Guid userPublisherId);
    }
}
