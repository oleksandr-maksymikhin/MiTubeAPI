using MiTube.BLL.DTO;
using MiTube.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IUsercredentialsService
    {
        Task<IEnumerable<UsercredentialsDTO>> GetAllAsync();
        Task<UsercredentialsDTO> GetByIdAsync(Guid id);
        Task<UsercredentialsDTO> CreateAsync(UsercredentialsDTO userDto);
        Task<UsercredentialsDTO> UpdateAsync(Guid id, UsercredentialsDTO usercredentialsDto);
        Task<UsercredentialsDTO> DeleteAsync(Guid id);

        Task<UserDTO> Login(UsercredentialsDTO usercredentialsDto);
        Task<UserDTO> Logout(string loggedUserId);
    }
}
