using MiTube.BLL.DTO;

namespace MiTube.BLL.Interfaces
{
    public interface IInteractionService
    {
        Task<IEnumerable<InteractionDTO>> GetAllAsync();
        Task<InteractionDTO?> GetByIdAsync(Guid id);
        Task<InteractionDTO?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId);
        Task<IEnumerable<InteractionDTO>?> GetByUserIdAsync(Guid userId);
        Task<InteractionDTO> CreateAsync(InteractionDTO interactionDTO);
        Task<InteractionDTO> UpdateAsync(Guid id, InteractionDTO interactionDTO);
        Task DeleteAsync(Guid id);
        Task DisposeAsync();
    }
}
