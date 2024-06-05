using MiTube.DAL.Entities;

namespace MiTube.DAL.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        public Task<Subscription?> GetByIdAsync(Guid id);
        public Task<IEnumerable<Subscription>?> GetAllBySubscriberId(Guid subscriberId);
        public Task<IEnumerable<Subscription>?> GetAllByPublisherId(Guid subscriberId);
        public Task<Subscription?> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId);

    }
}
