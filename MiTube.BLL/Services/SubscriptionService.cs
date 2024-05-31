using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IMapper mapper;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly IUserService userService;

        public SubscriptionService(IMapper mapper, ISubscriptionRepository subscriptionRepository, IUserService userService)
        {
            this.mapper = mapper;
            this.subscriptionRepository = subscriptionRepository;
            this.userService = userService;
        }

        public async Task<IEnumerable<SubscriptionDTO>> GetAllAsync()
        {
            IEnumerable<Subscription> subscriptions = await subscriptionRepository.GetAllAsync();
            if (subscriptions == null)
            {
                return Enumerable.Empty<SubscriptionDTO>();
            }
            IEnumerable<SubscriptionDTO> subscriptionsDtos = mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionDTO>>(subscriptions);
            return subscriptionsDtos;
        }

        public async Task<SubscriptionDTO?> GetByIdAsync(Guid id)
        {
            Subscription? subscription = await subscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
            {
                return null;
            }
            SubscriptionDTO subscriptionDto = mapper.Map<Subscription, SubscriptionDTO>(subscription);
            return subscriptionDto;
        }

        public async Task<SubscriptionDTO?> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId)
        {
            Subscription? subscription = await subscriptionRepository.GetByPublisherIdSubscriberIdAsync(publisherId, subscriberId);
            if (subscription == null)
            {
                return null;
            }
            SubscriptionDTO subscriptionDto = mapper.Map<Subscription, SubscriptionDTO>(subscription);
            return subscriptionDto;
        }

        public async Task<IEnumerable<UserDTO>?> GetSubscribersByPublisherIdAsync(Guid publisherId)
        {
            IEnumerable<Subscription>? subscriptionsByPublisherId = await subscriptionRepository.GetAllByPublisherId(publisherId);
            if (subscriptionsByPublisherId == null) 
            {
                return null;
            }
            List<UserDTO> subscribers = new List<UserDTO>();
            foreach (Subscription subscription in subscriptionsByPublisherId)
            {
                UserDTO? subscriber = await userService.GetByIdWithDetailsAsync(subscription.SubscriberId);
                if (subscriber != null)
                {
                    subscribers.Add(subscriber);
                }
            }
            return subscribers;
        }

        public async Task<IEnumerable<UserDTO>?> GetPublishersBySubscriberIdAsync(Guid subscriberId)
        {
            IEnumerable<Subscription>? subscriptionsByІubscriberId = await subscriptionRepository.GetAllBySubscriberId(subscriberId);
            if (subscriptionsByІubscriberId == null)
            {
                return null;
            }
            List<UserDTO> publishers = new List<UserDTO>();
            foreach (Subscription subscription in subscriptionsByІubscriberId)
            {
                UserDTO? publisher = await userService.GetByIdWithDetailsAsync(subscription.PublisherId);
                if (publisher != null)
                {
                    publishers.Add(publisher);
                }
            }
            return publishers;
        }

    public async Task<SubscriptionDTO?> CreateAsync(SubscriptionDTO subscriptionDtoToCreate)
        {
            //check Subscription exist
            SubscriptionDTO? subscriptionDtoExist = await GetByPublisherIdSubscriberIdAsync(subscriptionDtoToCreate.PublisherId, subscriptionDtoToCreate.SubscriberId);
            if (subscriptionDtoExist != null)
            {
                return subscriptionDtoExist;
            }

            //check UserId and VideoId
            UserDTO? userSubscriberDto = await userService.GetByIdAsync(subscriptionDtoToCreate.SubscriberId);
            UserDTO? userPublisherDto = await userService.GetByIdAsync(subscriptionDtoToCreate.PublisherId);
            if (userSubscriberDto == null || userPublisherDto == null)
            {
                return null;
                //return new SubscriptionDTO();
            }

            subscriptionDtoToCreate.Id = Guid.NewGuid();
            Subscription subscriptionToCreate = mapper.Map<SubscriptionDTO, Subscription>(subscriptionDtoToCreate);
            await subscriptionRepository.CreateAsync(subscriptionToCreate);
            SubscriptionDTO subscriptionDtoCreated = mapper.Map<Subscription, SubscriptionDTO>(subscriptionToCreate);
            return subscriptionDtoCreated;
        }


        public async Task<SubscriptionDTO?> UpdateAsync(Guid id, SubscriptionDTO subscriptionDto)
        {
            SubscriptionDTO? subscriptionDtoExist = await GetByIdAsync(subscriptionDto.Id);

            if (subscriptionDtoExist == null)
            {
                return null;
                //return new InteractionDTO();
            }

            Subscription subscriptionToUpdate = mapper.Map<SubscriptionDTO, Subscription>(subscriptionDtoExist);
            Subscription subscriptionUpdated = await subscriptionRepository.UpdateAsync(subscriptionToUpdate);
            SubscriptionDTO subscriptionDtoUpdated = mapper.Map<Subscription, SubscriptionDTO>(subscriptionUpdated);
            return subscriptionDtoUpdated;
        }

        public async Task DeleteAsync(Guid id)
        {
            Subscription? subscriptionExist = await subscriptionRepository.GetByIdAsync(id);
            if (subscriptionExist == null)
            {
                return;
            }
            await subscriptionRepository.DeleteAsync(subscriptionExist);
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
