using MiTube.BLL.DTO;

namespace MiTube.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        public Task<IEnumerable<UserDTO>> GetAllWithDetailsAsync();
        Task<UserDTO> GetByIdAsync(Guid id);
        public Task<UserDTO?> GetByIdWithDetailsAsync(Guid id);
        Task<UserDTO> CreateAsync(UserDTOCreateUpdate userDTOCreateUpdate);
        Task<UserDTO> UpdateAsync(Guid id, UserDTOCreateUpdate userDTOCreateUpdate);
        Task<UserDTO> DeleteAsync(Guid id);
        Task<IEnumerable<UserDTO>> SearchAsync(string search);
        Task DisposeAsync();
    }
}
