using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public Task<Comment?> GetByIdAsync(Guid id);
        public Task<Comment?> GetByIdWithVideoAsync(Guid id);
        public Task<Comment?> GetByIdWithUserAsync(Guid id);
        public Task<Comment?> GetByIdWithEverythingAsync(Guid id);
    }
}
