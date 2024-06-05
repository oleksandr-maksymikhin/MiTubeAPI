using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MiTube.BLL.DTO;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;

namespace MiTube.BLL.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly IMapper mapper;
        private readonly IInteractionRepository interactionRepository;
        private readonly IUserService userService;
        private readonly IVideoService videoService;

        Guid guidDefaultEmpty = Guid.Empty;

        public InteractionService(IMapper mapper, InteractionRepository interactionRepository, IUserService userService, IVideoService videoService)
        {
            this.mapper = mapper;
            this.interactionRepository = interactionRepository;
            this.userService = userService;
            this.videoService = videoService;
        }

        public async Task<IEnumerable<InteractionDTO>> GetAllAsync()
        {
            IEnumerable<DAL.Entities.Interaction> interactions = await interactionRepository.GetAllAsync();
            if (interactions == null)
            {
                return Enumerable.Empty<InteractionDTO>();
            }
            IEnumerable<InteractionDTO> interactionDtos = mapper.Map<IEnumerable<DAL.Entities.Interaction>, IEnumerable<InteractionDTO>>(interactions);
            return interactionDtos;
        }

        public async Task<InteractionDTO?> GetByIdAsync(Guid id)
        {
            DAL.Entities.Interaction? interaction = await interactionRepository.GetByIdAsync(id);
            if (interaction == null)
            {
                return new InteractionDTO();
            }
            InteractionDTO interactionDto = mapper.Map<DAL.Entities.Interaction, InteractionDTO>(interaction);
            return interactionDto;
        }

        public async Task<InteractionDTO?> GetByUserIdVideoIdAsync(Guid userId, Guid videoId)
        {
            DAL.Entities.Interaction? interaction = await interactionRepository.GetByUserIdVideoIdAsync(userId, videoId);
            if (interaction == null)
            {
                return null;
            }
            InteractionDTO interactionDto = mapper.Map<DAL.Entities.Interaction, InteractionDTO>(interaction);
            return interactionDto;
        }

        public async Task<IEnumerable<InteractionDTO>?> GetByUserIdAsync(Guid userId)
        {
            IEnumerable <DAL.Entities.Interaction>? interaction = await interactionRepository.GetByUserIdAsync(userId);
            if (interaction == null)
            {
                return null;
            }
            IEnumerable <InteractionDTO> interactionDto = mapper.Map<IEnumerable<DAL.Entities.Interaction>, IEnumerable<InteractionDTO>>(interaction);
            return interactionDto;
        }

        public async Task<InteractionDTO> CreateAsync(InteractionDTO interactionDtoToCreate)
        {
            InteractionDTO? interactionDtoExist = await GetByUserIdVideoIdAsync(interactionDtoToCreate.UserId, interactionDtoToCreate.VideoId);
            if (interactionDtoExist != null)
            {
                interactionDtoToCreate.Id = interactionDtoExist.Id;
                InteractionDTO interactionDtoUpdated = await UpdateAsync(interactionDtoToCreate.Id, interactionDtoToCreate);
                return interactionDtoUpdated;
            }

            UserDTO? userDto = await userService.GetByIdAsync(interactionDtoToCreate.UserId);
            VideoDTO? videoDto = await videoService.GetByIdAsync(interactionDtoToCreate.VideoId, guidDefaultEmpty);
            
            if (userDto == null || videoDto == null)
            {
                return new InteractionDTO();
            }

            interactionDtoToCreate.Id = Guid.NewGuid();
            DAL.Entities.Interaction interactionToCreate = mapper.Map<InteractionDTO, DAL.Entities.Interaction>(interactionDtoToCreate);
            interactionToCreate.Date = DateTime.Now;
            await interactionRepository.CreateAsync(interactionToCreate);
            InteractionDTO interactionDtoCreated = mapper.Map<DAL.Entities.Interaction, InteractionDTO>(interactionToCreate);
            return interactionDtoCreated;
        }

        public async Task<InteractionDTO> UpdateAsync(Guid id, InteractionDTO interactionDtoToUpdate)
        {
            InteractionDTO interactionDtoExist = await GetByIdAsync(interactionDtoToUpdate.Id);

            if (interactionDtoExist == null)
            {
                return null;
            }

            if (interactionDtoToUpdate.Actionstate == interactionDtoExist.Actionstate)
            {
                interactionDtoToUpdate.Actionstate = (int)InteractionStatusesEnum.viewed;
            }

            DAL.Entities.Interaction interactionToUpdate = mapper.Map<InteractionDTO, DAL.Entities.Interaction>(interactionDtoToUpdate);
            interactionToUpdate.Date = DateTime.Now;
            DAL.Entities.Interaction interactionUpdated = await interactionRepository.UpdateAsync(interactionToUpdate);
            InteractionDTO interactionDtoUpdated = mapper.Map<DAL.Entities.Interaction, InteractionDTO>(interactionUpdated);
            return interactionDtoUpdated;
        }

        public async Task DeleteAsync(Guid id)
        {
            DAL.Entities.Interaction? interactionExist = await interactionRepository.GetByIdAsync(id);
            if (interactionExist == null)
            {
                return;
            }
            await interactionRepository.DeleteAsync(interactionExist);
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
