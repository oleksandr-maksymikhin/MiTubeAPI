using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface IInteractionRepository : IRepository<Interaction>
    {
        public Task<Interaction?> GetByIdAsync(Guid id);
        public Task<Interaction?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId);
        public Task<IEnumerable<Interaction>?> GetByUserIdAsync(Guid userId);
    }
}
