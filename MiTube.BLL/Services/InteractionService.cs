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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //check Interaction exist
            InteractionDTO? interactionDtoExist = await GetByUserIdVideoIdAsync(interactionDtoToCreate.UserId, interactionDtoToCreate.VideoId);
            if (interactionDtoExist != null)
            {
                //return interactionDtoExist;
                interactionDtoToCreate.Id = interactionDtoExist.Id;
                InteractionDTO interactionDtoUpdated = await UpdateAsync(interactionDtoToCreate.Id, interactionDtoToCreate);
                return interactionDtoUpdated;
            }

            //check UserId and VideoId
            UserDTO? userDto = await userService.GetByIdAsync(interactionDtoToCreate.UserId);
            VideoDTO? videoDto = await videoService.GetByIdAsync(interactionDtoToCreate.VideoId, guidDefaultEmpty);
            
            if (userDto == null || videoDto == null)
            {
                //return null;
                return new InteractionDTO();
            }

            interactionDtoToCreate.Id = Guid.NewGuid();
            DAL.Entities.Interaction interactionToCreate = mapper.Map<InteractionDTO, DAL.Entities.Interaction>(interactionDtoToCreate);
            interactionToCreate.Date = DateTime.Now;
            await interactionRepository.CreateAsync(interactionToCreate);
            InteractionDTO interactionDtoCreated = mapper.Map<DAL.Entities.Interaction, InteractionDTO>(interactionToCreate);
            return interactionDtoCreated;
        }

        //////Create Interaction "view" once authorized user start to view the video
        /////Do not need this method because we moved Voew counter in InteractionController and POST views
        ///// !!!!!!!!!!!!!!!!!!!!
        //public async Task CreateInteractionViewAsync(Guid userId, Guid videoId)
        //{
        //    InteractionDTO interactionExist = await GetByUserIdVideoIdAsync(userId, videoId);
        //    if (interactionExist == null)
        //    {
        //        InteractionDTO createInteraction = new InteractionDTO();
        //        createInteraction.Id = Guid.NewGuid();
        //        createInteraction.UserId = userId;
        //        createInteraction.VideoId = videoId;
        //        createInteraction.Actionstate = (int)InteractionStatusesEnum.viewed;
        //        InteractionDTO createdInteraction = await CreateAsync(createInteraction);
        //    }
        //}

        public async Task<InteractionDTO> UpdateAsync(Guid id, InteractionDTO interactionDtoToUpdate)
        {
            InteractionDTO interactionDtoExist = await GetByIdAsync(interactionDtoToUpdate.Id);


            //UserDTO? userDto = await userService.GetByIdAsync(interactionDtoToUpdate.UserId);
            //VideoDTO? videoDto = await videoService.GetByIdAsync(interactionDtoToUpdate.VideoId);
            //if (interactionDtoExist == null || userDto == null || videoDto == null)
            if (interactionDtoExist == null)
            {
                return null;
                //return new InteractionDTO();
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

        //public async Task<IEnumerable<InteractionDTO>> FindInteractionByUserIdAsync(Guid userId)
        //{
        //    IEnumerable<DAL.Entities.Interaction> interactionsByUserId = interactionRepository.FindByCondition(interaction => interaction.UserId == userId);

        //    IEnumerable<InteractionDTO> interactionsDtoByUserId = mapper.Map<IEnumerable<DAL.Entities.Interaction>, IEnumerable<InteractionDTO>>(interactionsByUserId);

        //    return interactionsDtoByUserId;
        //}

        //public async Task<IEnumerable<InteractionDTO>> FindInteractionByVideoIdAsync(Guid videoId)
        //{
        //    IEnumerable<DAL.Entities.Interaction> interactionsByVideoId = interactionRepository.FindByCondition(interaction => interaction.VideoId == videoId);

        //    IEnumerable<InteractionDTO> interactionsDtoByVideoId = mapper.Map<IEnumerable<DAL.Entities.Interaction>, IEnumerable<InteractionDTO>>(interactionsByVideoId);

        //    return interactionsDtoByVideoId;
        //}

        //public async Task<IEnumerable<InteractionDTO>> FindInteractionByUserIdVideoIdAsync(Guid userId, Guid videoId)
        //{
        //    IEnumerable<DAL.Entities.Interaction> interactionsByUserId = interactionRepository.FindByCondition(interaction => interaction.UserId == userId && interaction.VideoId == videoId);

        //    IEnumerable<InteractionDTO> interactionsDtoByUserId = mapper.Map<IEnumerable<DAL.Entities.Interaction>, IEnumerable<InteractionDTO>>(interactionsByUserId);

        //    return interactionsDtoByUserId;
        //}


    }
}
