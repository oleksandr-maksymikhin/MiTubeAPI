using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Repositories;
using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiTube.BLL.Infrastructure;
using MiTube.DAL.Interfaces;

namespace MiTube.BLL.Services
{
    public class VideoService : IVideoService

    {
        private readonly IMapper mapper;
        private readonly IVideoRepository videoRepository;
        private readonly IUserService userService;
        //private readonly IInteractionService interactionService;                  //circular dependency VideoService <> InteractionService
        private readonly IInteractionRepository interactionRepository;
        //BlobServiceClient blobServiceClient;
        //string blobContainerName = "mitube";

        private readonly IBlobProcessingService blobProcessingService;

        Guid guidDefaultEmpty = Guid.Empty;

        //private readonly IPlaylistService playlistService;
        private readonly PlaylistRepository playlistRepository;

        //public VideoService(IMapper mapper, VideoRepository videoRepository, UserService userService, InteractionRepository interactionRepository, BlobServiceClient blobServiceClient)
        //public VideoService(IMapper mapper, VideoRepository videoRepository, UserService userService, InteractionRepository interactionRepository, BlobProcessingService blobProcessingService)
        //public VideoService(IMapper mapper, VideoRepository videoRepository, UserService userService, InteractionRepository interactionRepository, BlobProcessingService blobProcessingService, PlaylistService playlistService)
        public VideoService(IMapper mapper, VideoRepository videoRepository, UserService userService, InteractionRepository interactionRepository, BlobProcessingService blobProcessingService, PlaylistRepository playlistRepository)
        {
            this.mapper = mapper;
            this.videoRepository = videoRepository;
            this.userService = userService;
            //this.interactionService = interactionService;
            this.interactionRepository = interactionRepository;
            //this.blobServiceClient = blobServiceClient;
            //mapper = new MapperConfiguration(cfg => cfg.CreateMap<VideoDTO, Video>().ReverseMap()).CreateMapper();

            this.blobProcessingService = blobProcessingService;
            //this.playlistService = playlistService;
            this.playlistRepository = playlistRepository;
        }

        public async Task<IEnumerable<VideoDTO>> GetAllAsync(Guid userId)
        {
            IEnumerable<Video> videos = await videoRepository.GetAllAsync();
            //// ************* Check if it is not null ************** ///////
            ///IEnumerable<VideoDTO> videoDtos = mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
            List<VideoDTO> videoDtos = new List<VideoDTO>();

            foreach (Video video in videos)
            {
                VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        public async Task<IEnumerable<VideoDTO>> GetAllWithDetailsAsync(Guid userId)
        {
            
            IEnumerable<Video> videos = await videoRepository.GetAllWithDetailsAsync();
            //List<Video> videos = (await videoRepository.GetAllWithDetailsAsync()).ToList();

            //Video videoDD = await videoRepository.GetAllWithDetailsAsync();
            //List<Video> videos = new List<Video>();
            //videos.Add(videoDD);

            //// ************* Check if it is not null ************** ///////
            ///IEnumerable<VideoDTO> videoDtos = mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
            List<VideoDTO> videoDtos = new List<VideoDTO>();

            foreach (Video video in videos)
            {
                VideoDTO videoDto = await CreateVideoDTOFromVideoTypeThinUser(video, userId);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        public async Task<VideoDTO> GetByIdAsync(Guid id, Guid userId)
        {
            Video video = await videoRepository.GetByIdAsync(id);
            //// ************* Check if it is not null ************** ///////
            VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
            videoDto.User = await userService.GetByIdAsync(video.UserId);
            return videoDto;
        }
        public async Task<VideoDTO> GetByIdWithDetailsAsync(Guid id, Guid userId)
        {
            Video? videos = await videoRepository.GetByIdWithDetailsAsync(id);
            //// ************* Check if it is not null ************** ///////
            VideoDTO videoDto = await CreateVideoDTOFromVideoType(videos, userId);
            return videoDto;
        }

        public async Task<IEnumerable<VideoDTO>> GetByUserIdWithDetailsAsync(Guid userPublisherId, Guid userId)
        {
            IEnumerable<Video>videos = await videoRepository.GetByUserIdWithDetailsAsync(userPublisherId);
            //// ************* Check if it is not null ************** ///////
            List<VideoDTO> videoDtos = new List<VideoDTO>();

            foreach (Video video in videos)
            {
                VideoDTO videoDto = await CreateVideoDTOFromVideoTypeThinUser(video, userId);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        public async Task<IEnumerable<VideoDTO>> SearchVideoAsync(String? search, Guid userId)
        {
            if (search == null)
            {
                IEnumerable<VideoDTO> videosDTO = await GetAllAsync(userId);
                return videosDTO;
            }
            IEnumerable<Video> videos = await videoRepository.SearchAsync(search);
            //// ************* Check if it is not null ************** ///////
            ///IEnumerable<VideoDTO> videoDtos = mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
            List<VideoDTO> videoDtos = new List<VideoDTO>();

            foreach (Video video in videos)
            {
                VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        public async Task<IEnumerable<VideoDTO>> SearchVideoWithDetailsAsync(String search, Guid userId)
        {
            //if (search == null)
            //{
            //    IEnumerable<VideoDTO> videosDTO = await GetAllWithDetailsAsync(userId);
            //    return videosDTO;
            //}
            IEnumerable<Video> videos = await videoRepository.SearchWithDetailsAsync(search);
            //// ************* Check if it is not null ************** ///////
            ///IEnumerable<VideoDTO> videoDtos = mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
            List<VideoDTO> videoDtos = new List<VideoDTO>();

            foreach (Video video in videos)
            {
                VideoDTO videoDto = await CreateVideoDTOFromVideoTypeThinUser(video, userId);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        // !!!!!!!!!!!!!!!! after completion of InteractionService -> complete this method !!!!!!!!!!!!!!!!!!!!!!
        private async Task<VideoDTO> CreateVideoDTOFromVideoType(Video video, Guid userId)
        {
            VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
            videoDto.User = await userService.GetByIdWithDetailsAsync(video.UserId);
            videoDto.Likecount = video.Interactions.Count(i => i.Actionstate == (int)InteractionStatusesEnum.liked);
            videoDto.Dislikecount = video.Interactions.Count(i => i.Actionstate == (int)InteractionStatusesEnum.disliked);
            videoDto.Views = video.Interactions.Count();
            videoDto.Commentscount = video.Comments.Count();
            videoDto.LikeRate = videoDto.Views != 0 
                ? (float)videoDto.Likecount / videoDto.Views 
                : 0;
            if (userId != Guid.Empty)
            {
                //InteractionDTO interactionDtoExist = await interactionService.GetByUserIdVideoIdAsync(userId, video.Id);      //circular dependency VideoService <> InteractionService
                //Interaction interactionExist = await interactionRepository.GetByUserIdVideoIdAsync(userId, video.Id);           //work but direct injection of InteractionRepository into VideoService
                Interaction? interactionExist = video.Interactions.FirstOrDefault(i => i.UserId == userId && i.VideoId == video.Id);
                if (interactionExist != null)
                {
                    videoDto.LikeStatus = interactionExist.Actionstate;
                }
                
            }
            
            return videoDto;
        }


        private async Task<VideoDTO> CreateVideoDTOFromVideoTypeThinUser(Video video, Guid userId)
        {
            VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
            videoDto.User = await userService.GetByIdAsync(video.UserId);
            videoDto.Likecount = video.Interactions.Count(i => i.Actionstate == (int)InteractionStatusesEnum.liked);
            videoDto.Dislikecount = video.Interactions.Count(i => i.Actionstate == (int)InteractionStatusesEnum.disliked);
            videoDto.Views = video.Interactions.Count();
            videoDto.Commentscount = video.Comments.Count();
            videoDto.LikeRate = videoDto.Views != 0
                ? (float)videoDto.Likecount / videoDto.Views
                : 0;
            if (userId != Guid.Empty)
            {
                //InteractionDTO interactionDtoExist = await interactionService.GetByUserIdVideoIdAsync(userId, video.Id);      //circular dependency VideoService <> InteractionService
                //Interaction interactionExist = await interactionRepository.GetByUserIdVideoIdAsync(userId, video.Id);           //work but direct injection of InteractionRepository into VideoService
                Interaction? interactionExist = video.Interactions.FirstOrDefault(i => i.UserId == userId && i.VideoId == video.Id);
                if (interactionExist != null)
                {
                    videoDto.LikeStatus = interactionExist.Actionstate;
                }

            }

            return videoDto;
        }

        //public Task<VideoDTO> CreateAsync(VideoDTOCreate createVideoDto)
        public async Task<VideoDTO> CreateAsync(VideoDTOCreate videoDTOCreate)
        {
            // !!!!!!!!!! move it somewhere in DI

            //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            ////blobContainerClient = serviceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = await blobContainerClient.ExistsAsync();
            //if (!isExist)
            //{
            //    blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //}
            try
            {
                //String videoUrl = await UploadFile(videoDTOCreate.VideoFile, blobContainerClient);
                //String posterUrl = await UploadFile(videoDTOCreate.PosterFile, blobContainerClient);

                String videoUrl = await blobProcessingService.UploadFile(videoDTOCreate.VideoFile);
                String posterUrl = await blobProcessingService.UploadFile(videoDTOCreate.PosterFile);

                //!!!!!!!!!!!!!!!! problem in mapper !!!!!!!!!!!!!!!!!!!
                //Video newVideo = mapper.Map<VideoDTOCreate, Video>(videoDTOCreate);
                Video videoNew = new Video();
                videoNew.Id = Guid.NewGuid();

                videoNew.UserId = videoDTOCreate.UserId;
                videoNew.Title = videoDTOCreate.Title;
                videoNew.Description = videoDTOCreate.Description;
                videoNew.IsPublic = videoDTOCreate.IsPublic;
                videoNew.Date = DateTime.Now;
                videoNew.Duration = videoDTOCreate.Duration;

                videoNew.VideoUrl = videoUrl;
                videoNew.PosterUrl= posterUrl;

                ////////////////////////////////////
                await videoRepository.CreateAsync(videoNew);

                //add video to Playlist -->>> use PlaylistRepository because of circular DI VideoService PlaylistService 
                if (videoDTOCreate.PlaylistId != null)
                {
                    Guid newGuid = Guid.Parse(videoDTOCreate?.PlaylistId?.ToString());
                    Guid playlistId = (await playlistRepository?.GetByIdAsync(newGuid)).Id;
                    playlistRepository.AddVideo(playlistId, videoNew);
                }


                VideoDTO createdVideoDTO = mapper.Map<Video, VideoDTO>(videoNew);
                createdVideoDTO.User = await userService.GetByIdAsync(videoNew.UserId);
                return createdVideoDTO;
            }
            catch (Exception)
            {

                throw;
            }

            throw new NotImplementedException();
        }

        //private async Task<string> UploadFile(IFormFile fileToUpload, BlobContainerClient blobContainerClient)
        //{
        //    string blobName = fileToUpload.FileName;
        //    BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
        //    using (var fileStream = fileToUpload.OpenReadStream())
        //    {
        //        await blobClient.UploadAsync(fileStream, true);
        //    }
        //    string downloadUrl = blobClient.Uri.AbsoluteUri;
        //    return downloadUrl;
        //}

        public async Task<VideoDTO> UpdateAsync(Guid id, VideoDTOUpdate videoDtoUpdate)
        {
            // !!!!!!!!!! move it somewhere in DI
            //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = await blobContainerClient.ExistsAsync();
            //if (!isExist)
            //{
            //    blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //}

            VideoDTO videoDtoExist = await GetByIdAsync(id, Guid.NewGuid());

            if (videoDtoExist == null)
            {
                return null;
            }

            try
            {
                String videoUrl = videoDtoExist.VideoUrl;
                String posterUrl = videoDtoExist.PosterUrl;

                //BlobClient blobClient = new BlobClient(new Uri(videoDtoExist.VideoUrl));
                //String videoName = blobClient.Name;
                //BlobClient blobClient = new BlobClient(new Uri(videoDtoExist.PosterUrl));
                //String posterName = new BlobClient(new Uri(videoDtoExist.PosterUrl)).Name;

                //if (videoName != videoDtoUpdate.VideoFile.FileName && videoDtoUpdate.VideoFile != null)
                //{
                //    await RemoveFile(videoDtoExist.VideoUrl, blobContainerClient);
                //    videoUrl = await UploadFile(videoDtoUpdate.VideoFile, blobContainerClient);
                //}
                //if (posterName != videoDtoUpdate.PosterFile.FileName && videoDtoUpdate.PosterFile != null)
                //{
                //    await RemoveFile(videoDtoExist.PosterUrl, blobContainerClient);
                //    posterUrl = await UploadFile(videoDtoUpdate.PosterFile, blobContainerClient);
                //}

                ////!!!!!!!!!! We do not update video? only delete
                //if (videoName != videoDtoUpdate.VideoFile.FileName && videoDtoUpdate.VideoFile != null)
                //{
                //    await blobProcessingService.RemoveFile(videoDtoExist.VideoUrl);
                //    videoUrl = await blobProcessingService.UploadFile(videoDtoUpdate.VideoFile);
                //}

                //if (posterName != videoDtoUpdate.PosterFile.FileName && videoDtoUpdate.PosterFile != null)
                if (videoDtoUpdate.PosterFile != null)
                {
                    posterUrl = await blobProcessingService.UploadFile(videoDtoUpdate.PosterFile);
                    if (videoDtoExist.PosterUrl != null)
                    {
                        await blobProcessingService.RemoveFile(videoDtoExist.PosterUrl);
                    }
                }

                //!!!!!!!!!!!!!!!!!!! error in mapping !!!!!!!!!!!!!!!!!!
                //Video videoToUpdate = mapper.Map<VideoDTOCreate, Video>(videoDtoUpdate);

        Video videoToUpdate = new Video();

                videoToUpdate.Id = videoDtoExist.Id;
                videoToUpdate.UserId = videoDtoExist.User.Id;
                //if (videoDtoUpdate.Title == null)
                //{
                //    videoToUpdate.Title = videoDtoExist.Title;
                //}
                //videoToUpdate.Title = videoDtoUpdate.Title;

                videoToUpdate.Title = videoDtoUpdate.Title == null ? videoDtoExist.Title : videoDtoUpdate.Title;

                videoToUpdate.Description = videoDtoUpdate.Description == null ? videoDtoExist.Description : videoDtoUpdate.Description;
                videoToUpdate.IsPublic = videoDtoUpdate.IsPublic;
                videoToUpdate.Date = DateTime.Now;
                videoToUpdate.Duration = videoDtoExist.Duration;

                videoToUpdate.VideoUrl = videoDtoExist.VideoUrl;
                videoToUpdate.PosterUrl = posterUrl;

                await videoRepository.UpdateAsync(videoToUpdate);

                VideoDTO videoDtoUpdated = mapper.Map<Video, VideoDTO>(videoToUpdate);
                videoDtoUpdated.User = await userService.GetByIdAsync(videoToUpdate.UserId);

                return videoDtoUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VideoDTO> DeleteAsync(Guid id)
        {
            // !!!!!!!!!!!!!!!! consider if it is OK to call VideoRepository from this method ???? 

            VideoDTO videoDtoToDelete = await GetByIdAsync(id, Guid.NewGuid());
            if (videoDtoToDelete == null)
            {
                return null;
            }
            Video video = mapper.Map<VideoDTO, Video>(videoDtoToDelete);

            //Video video = await videoRepository.GetByIdAsync(id);

            // !!!!!!!!!! move it somewhere in DI
            //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = await blobContainerClient.ExistsAsync();
            //if (!isExist)
            //{
            //    blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //}

            //if (video.VideoUrl != null)
            //{
            //    await RemoveFile(video.VideoUrl, blobContainerClient);
            //}
            //if (video.PosterUrl != null)
            //{
            //    await RemoveFile(video.PosterUrl, blobContainerClient);
            //}

            if (video.VideoUrl != null)
            {
                await blobProcessingService.RemoveFile(video.VideoUrl);
            }
            if (video.PosterUrl != null)
            {
                await blobProcessingService.RemoveFile(video.PosterUrl);
            }

            await videoRepository.DeleteAsync(video);
            return videoDtoToDelete;
        }

        //private async Task RemoveFile(string url, BlobContainerClient blobContainerClient)
        //{
        //    BlobClient blobClient = new BlobClient(new Uri(url));
        //    String blobName = blobClient.Name;
        //    blobClient = blobContainerClient.GetBlobClient(blobName);
        //    await blobClient.DeleteIfExistsAsync();
        //    return;
        //}

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VideoDTO>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }


    }
}
