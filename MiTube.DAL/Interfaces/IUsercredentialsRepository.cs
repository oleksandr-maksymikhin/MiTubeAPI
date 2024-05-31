using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Interfaces
{
    public interface IUsercredentialsRepository
    {
        public Task<Usercredentials?> GetByIdAsync(Guid id);
        public Task<Usercredentials?> SearchByEmailAsync(String email);
        public Task<Usercredentials?> SearchEntityAsync(Usercredentials usercredentials);
    }
}
