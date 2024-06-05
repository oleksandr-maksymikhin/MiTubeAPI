using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface IUsercredentialsRepository : IRepository<Usercredentials>
    {
        public Task<Usercredentials?> GetByIdAsync(Guid id);
        public Task<Usercredentials?> SearchByEmailAsync(String email);
        public Task<Usercredentials?> SearchEntityAsync(Usercredentials usercredentials);
    }
}
