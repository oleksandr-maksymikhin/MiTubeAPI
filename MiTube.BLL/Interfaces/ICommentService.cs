using MiTube.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface ICommentService
    {
        public Task<IEnumerable<CommentDTO>> GetAllAsync();

        public Task<CommentDTO?> GetByIdAsync(Guid id);

        public Task<CommentDTO?> CreateAsync(CommentDTO commentDto);
        
        public Task<CommentDTO?> UpdateAsync(CommentDTO commentDto);

        public Task DeleteAsync(Guid id);
        
        public Task DisposeAsync();

        public Task<IEnumerable<CommentDTO>> GetByVideoIdAsync(Guid id);
        public Task<IEnumerable<CommentDTO>> GetByUserIdAsync(Guid id);
    }
}
