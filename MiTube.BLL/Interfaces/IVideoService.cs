using Microsoft.AspNetCore.Http;
using MiTube.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IVideoService
    {
        public Task<IEnumerable<VideoDTO>> GetAllAsync(Guid userId);
        public Task<IEnumerable<VideoDTO>> GetAllWithDetailsAsync(Guid userId);
        public Task<VideoDTO> GetByIdAsync(Guid id, Guid userId);
        public Task<VideoDTO> GetByIdWithDetailsAsync(Guid id, Guid userId);
        public Task<IEnumerable<VideoDTO>> GetByUserIdWithDetailsAsync(Guid userPublisherId, Guid userId);
        public Task<IEnumerable<VideoDTO>> SearchVideoAsync(String search, Guid userId);
        public Task<IEnumerable<VideoDTO>> SearchVideoWithDetailsAsync(String? search, Guid userId);
        //Task<VideoDTO> CreateAsync(VideoDTO videoDto);
        public Task<VideoDTO> CreateAsync(VideoDTOCreate videoDtoCreate);
        public Task<VideoDTO> UpdateAsync(Guid id, VideoDTOUpdate videoDtoUpdate);
        public Task<VideoDTO> DeleteAsync(Guid id);

        public Task<IEnumerable<VideoDTO>> SearchAsync(string search);
        public Task DisposeAsync();
    }
}
