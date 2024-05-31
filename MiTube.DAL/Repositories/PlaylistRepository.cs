using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Repositories
{
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(DbContext context) : base(context)
        {
        }

        public void AddVideo(Guid id, Video video)
        {
            Playlist? pl = dbSet.Single(p => p.Id == id);
            
            dbSet.Entry(pl)
            .Collection(p => p.Videos)
            .Load();

            pl.Videos.Add(video);
            Save();
        }

        public async Task<IEnumerable<Playlist>> GetAllWithDetailsAsync()
        {
            return await Task.Run(() => dbSet
                .Include(pl => pl.Videos)
                .Include(pl => pl.User).ToList());
        }

        public async Task<Playlist?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(e => e.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Playlist?> GetByIdWithEverythingAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.Videos)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Playlist?> GetByIdWithUserAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Playlist?> GetByIdWithVideosAsync(Guid id)
        {
            return await FindByCondition(video => video.Id.Equals(id))
                .Include(e => e.Videos)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Playlist>> GetByUserIdWithDetailsAsync(Guid userId)
        {
            return await Task.Run(() => FindByCondition(playList => playList.UserId.Equals(userId))
                .Include(pl => pl.Videos)
                .Include(pl => pl.User).ToList());
        }

        //public async Task<Playlist?> GetWatchLaterPlaylistByUserIdAsync(Guid userId)
        //{
        //    return await Task.Run(() => FindByCondition(playList => playList.UserId.Equals(userId) && playList.Name.Contains("watch later"))
        //            .FirstOrDefaultAsync());
        //}

    }
}
