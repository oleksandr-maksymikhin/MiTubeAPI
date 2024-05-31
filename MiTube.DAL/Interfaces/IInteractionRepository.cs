using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Interfaces
{
    public interface IInteractionRepository : IRepository<Interaction>
    {
        //public Interaction GetById(Guid id);
        public Task<Interaction?> GetByIdAsync(Guid id);
        public Task<Interaction?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId);
        public Task<IEnumerable<Interaction>?> GetByUserIdAsync(Guid userId);
    }
}
