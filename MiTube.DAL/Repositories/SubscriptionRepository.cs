using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.DAL.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DbContext context) : base(context)
        {
        }

        public async Task<Subscription?> GetByIdAsync(Guid id)
        {
            return await FindByCondition(subscription => subscription.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Subscription>?> GetAllByPublisherId(Guid publisherId)
        {
            return await Task.Run(() => FindByCondition(subscription => subscription.PublisherId.Equals(publisherId))
                .Include(s => s.Subscriber)
                .Include(p => p.Publisher).ToList());
        }

        public async Task<IEnumerable<Subscription>?> GetAllBySubscriberId(Guid subscriberId)
        {
            return await Task.Run(() => FindByCondition(subscription => subscription.SubscriberId.Equals(subscriberId))
                .Include(s => s.Subscriber)
                .Include(p => p.Publisher).ToList());
        }

        public async Task<Subscription?> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId)
        {
            return await FindByCondition(subscription => subscription.PublisherId.Equals(publisherId) && subscription.SubscriberId.Equals(subscriberId))
                       .FirstOrDefaultAsync();
        }

    }
}
