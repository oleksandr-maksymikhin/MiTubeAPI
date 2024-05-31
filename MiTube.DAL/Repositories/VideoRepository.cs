using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class VideoRepository: GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(DbContext context) : base(context)
        {
        }

        //// not in use
        //public Video GetById(Guid id)
        //{
        //    return dbSet
        //        .AsNoTracking()
        //        .FirstOrDefault(t => t.Id == id);
        //}


        //without ToLst() there is an ERROR in SQLServer in Azure !!!, but there was no error in local SQL Server ???
        public async Task<IEnumerable<Video>> GetAllWithDetailsAsync()
        {
            return await Task.Run(() => GetAll()
                .Include(c => c.Comments)
                .Include(i => i.Interactions).ToList());

            //return GetAll()
            //        .Include(c => c.Comments)
            //        .Include(i => i.Interactions);
            //.FirstOrDefaultAsync();


            //return dbSet
            //        .AsNoTracking()
            //        .Include(c => c.Comments)
            //        .Include(i => i.Interactions);


            //return await Task.Run(() => dbSet
            //            .AsNoTracking()
            //            .Include(c => c.Comments)
            //            .Include(i => i.Interactions));
        }

        public async Task<Video?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                        .FirstOrDefaultAsync();
        }

        public async Task<Video?> GetByIdWithDetailsAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions)
                        .FirstOrDefaultAsync();
        }

        //add ToList()
        public async Task<IEnumerable<Video>?> GetByUserIdWithDetailsAsync(Guid userId)
        {
            return await Task.Run(() => FindByCondition(video => video.UserId.Equals(userId))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions).ToList());
        }

        //add ToList()
        public async Task<IEnumerable<Video>> SearchAsync(String search)
        {
            return await Task.Run(() => FindByCondition(video => video.Description
                .Contains(search) || video.Title.Contains(search)).ToList());
        }

        //add ToList()
        public async Task<IEnumerable<Video>> SearchWithDetailsAsync(String search)
        {
            return await Task.Run(() => FindByCondition(video => video.Description
                .Contains(search) || video.Title.Contains(search))
                    .Include(c => c.Comments)
                    .Include(i => i.Interactions).ToList());
        }
    }
}
