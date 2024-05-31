using MiTube.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IUserService
    {
        //IEnumerable<UserDTO> GetAllUsers();
        Task<IEnumerable<UserDTO>> GetAllAsync();
        public Task<IEnumerable<UserDTO>> GetAllWithDetailsAsync();
        //UserDTO GetUserById(Guid id);
        Task<UserDTO> GetByIdAsync(Guid id);
        public Task<UserDTO?> GetByIdWithDetailsAsync(Guid id);
        //void CreateUser(UserDTO userDto);
        Task<UserDTO> CreateAsync(UserDTOCreateUpdate userDTOCreateUpdate);
        //void UpdateUser(UserDTO userDto);
        Task<UserDTO> UpdateAsync(Guid id, UserDTOCreateUpdate userDTOCreateUpdate);
        //void DeleteUser(UserDTO userDto);
        Task<UserDTO> DeleteAsync(Guid id);
        //IEnumerable<UserDTO> SearchUsers(string search);
        
        
        
        Task<IEnumerable<UserDTO>> SearchAsync(string search);
        //void Dispose();
        Task DisposeAsync();
    }
}
