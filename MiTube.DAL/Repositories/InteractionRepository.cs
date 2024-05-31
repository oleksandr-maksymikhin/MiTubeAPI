using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class InteractionRepository : GenericRepository<Interaction>, IInteractionRepository
    {
        public InteractionRepository(DbContext context) : base(context)
        { 
        }

        //public Interaction GetById(Guid id)
        //{
        //    return dbSet.AsNoTracking().FirstOrDefault(t => t.Id == id);
        //}

        public async Task<Interaction?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                       .FirstOrDefaultAsync();
        }

        public async Task<Interaction?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId)
        {
            return await FindByCondition(interaction => interaction.UserId.Equals(userId) && interaction.VideoId.Equals(videoId))
                       .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Interaction>?> GetByUserIdAsync(Guid userId)
        {
            return await Task.Run(() => FindByCondition(interaction => interaction.UserId.Equals(userId)).ToList());
        }



        ////get Interaction by Id
        //public Interaction GetInteractionByInteractionId(Guid interactionId)
        //{
        //    return FindByCondition(interaction => interaction.Id.Equals(interactionId)).FirstOrDefault();
        //}

        //public async Task<Interaction> GetInteractionByInteractionIdAsync(Guid interactionId)
        //{
        //    return await FindByCondition(interaction => interaction.Id.Equals(interactionId)).FirstOrDefaultAsync();
        //}

        ////get collection Interaction by Id synchronously 
        //public IEnumerable<Interaction> GetInteractionByUserIdEnumerable(Guid userId)
        //{
        //    return FindByCondition(interaction => interaction.UserId.Equals(userId)).ToList<Interaction>();
        //}

        //public IQueryable<Interaction> GetInteractionByUserIdQueryable(Guid userId)
        //{
        //    return FindByCondition(interaction => interaction.UserId.Equals(userId));
        //}

        ////get collection Interaction by Id asynchronously 
        //public async Task <IEnumerable<Interaction>> GetInteractionByUserIdEnumerableAsync(Guid userId)
        //{
        //    return await FindByCondition(interaction => interaction.UserId.Equals(userId)).ToListAsync<Interaction>();
        //}

        ////public async Task<IQueryable<Interaction>> GetInteractionByUserIdQueryableAsync(Guid userId)
        ////{
        ////    return await Task.Run(() => GetInteractionByUserIdQueryable(userId));
        ////}

        //public async Task<IQueryable<Interaction>> GetInteractionByUserIdQueryableAsync(Guid userId)
        //{
        //    return await Task.Run(() => FindByCondition(interaction => interaction.UserId.Equals(userId)));
        //}

        ////public async Task<IQueryable<Interaction>> GetInteractionByUserIdQueryableAsync(Guid userId)
        ////{
        ////    return await FindByCondition(interaction => interaction.UserId.Equals(userId)); ;
        ////}

    }
}