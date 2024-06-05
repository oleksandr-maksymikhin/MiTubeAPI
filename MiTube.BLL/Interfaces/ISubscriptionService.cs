using MiTube.BLL.DTO;

namespace MiTube.BLL.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDTO>> GetAllAsync();
        Task<SubscriptionDTO?> GetByIdAsync(Guid id);
        Task<SubscriptionDTO?> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId);
        Task<IEnumerable<UserDTO>?> GetSubscribersByPublisherIdAsync(Guid publisherId);
        Task<IEnumerable<UserDTO>?> GetPublishersBySubscriberIdAsync(Guid subscriberId);
        Task<SubscriptionDTO?> CreateAsync(SubscriptionDTO subscriptionDto);
        Task<SubscriptionDTO?> UpdateAsync(Guid id, SubscriptionDTO subscriptionDto);
        Task DeleteAsync(Guid id);
        Task DisposeAsync();
    }
}
