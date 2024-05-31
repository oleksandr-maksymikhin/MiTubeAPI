using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        public Task<Subscription?> GetByIdAsync(Guid id);
        //public Task<Subscription>? GetByUserIdVideoIdAsync(Guid userId, Guid videoId);
        public Task<IEnumerable<Subscription>?> GetAllBySubscriberId(Guid subscriberId);
        public Task<IEnumerable<Subscription>?> GetAllByPublisherId(Guid subscriberId);
        public Task<Subscription?> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId);

    }
}
