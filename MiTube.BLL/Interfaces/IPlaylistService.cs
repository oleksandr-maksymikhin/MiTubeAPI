using MiTube.BLL.DTO;

namespace MiTube.BLL.Interfaces
{
    public interface IPlaylistService
    {
        public Task<IEnumerable<PlaylistDTO>> GetAllAsync();
        public Task<PlaylistDTO?> GetByIdAsync(Guid id);
        public Task<PlaylistDTO?> GetByIdWithDetailsAsync(Guid id);
        public Task<PlaylistDTO?> CreateAsync(PlaylistDTOCreate playlistDtoCreate);
        public Task<PlaylistDTO?> UpdateAsync(PlaylistDTOCreate playlistDTOUpdate);
        public Task DeleteAsync(Guid id);
        public Task DisposeAsync();
        public Task<IEnumerable<VideoDTO>> GetVideosByIdAsync(Guid id);
        public Task AddVideoAsync(Guid playlistId, Guid videoId);
        public Task<IEnumerable<PlaylistDTO>> GetByUserIdWithDetailsAsync(Guid userId);

    }
}
