using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public Task<Playlist?> GetWatchLaterPlaylistByUserIdAsync(Guid userId);
    }
}
