using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IInteractionService
    {
        Task<IEnumerable<InteractionDTO>> GetAllAsync();
        Task<InteractionDTO?> GetByIdAsync(Guid id);
        Task<InteractionDTO?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId);
        Task<IEnumerable<InteractionDTO>?> GetByUserIdAsync(Guid userId);
        Task<InteractionDTO> CreateAsync(InteractionDTO interactionDTO);
        //Task CreateInteractionViewAsync(Guid userId, Guid videoId);
        Task<InteractionDTO> UpdateAsync(Guid id, InteractionDTO interactionDTO);
        Task DeleteAsync(Guid id);
        Task DisposeAsync();

        //public Task<IEnumerable<InteractionDTO>> GetByVideoId(Guid id);
        //public Task<IEnumerable<InteractionDTO>> GetByUserId(Guid id);

        //Task<IEnumerable<InteractionDTO>> FindInteractionByUserIdAsync(Guid userId);
        //Task<IEnumerable<InteractionDTO>> FindInteractionByVideoIdAsync(Guid videoId);
        //Task<IEnumerable<InteractionDTO>> FindInteractionByUserIdVideoIdAsync(Guid userId, Guid videoId);

    }
}
