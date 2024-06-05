using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Repositories;
using MiTube.DAL.Entities;
using MiTube.BLL.Infrastructure;
using MiTube.DAL.Interfaces;

namespace MiTube.BLL.Services
{
    public class VideoService : IVideoService

    {
        private readonly IMapper mapper;
        private readonly IVideoRepository videoRepository;
        private readonly IUserService userService;
        private readonly IInteractionRepository interactionRepository;
        private readonly IBlobProcessingService blobProcessingService;
        private readonly PlaylistRepository playlistRepository;
        private Guid guidDefaultEmpty = Guid.Empty;
        public VideoService(IMapper mapper, 
                            VideoRepository videoRepository, 
                            UserService userService, 
                            InteractionRepository interactionRepository, 
                            BlobProcessingService blobProcessingService, 
                            PlaylistRepository playlistRepository)
        {
            this.mapper = mapper;
            this.videoRepository = videoRepository;
            this.userService = userService;
            this.interactionRepository = interactionRepository;
            this.blobProcessingService = blobProcessingService;
            this.playlistRepository = playlistRepository;
        }

        public async Task<IEnumerable<VideoDTO>> GetAllAsync(Guid userId)
        {
            IEnumerable<Video> videos = await videoRepository.GetAllAsync();
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
            VideoDTO videoDto = mapper.Map<Video, VideoDTO>(video);
            videoDto.User = await userService.GetByIdAsync(video.UserId);
            return videoDto;
        }
        public async Task<VideoDTO> GetByIdWithDetailsAsync(Guid id, Guid userId)
        {
            Video? videos = await videoRepository.GetByIdWithDetailsAsync(id);
            VideoDTO videoDto = await CreateVideoDTOFromVideoType(videos, userId);
            return videoDto;
        }

        public async Task<IEnumerable<VideoDTO>> GetByUserIdWithDetailsAsync(Guid userPublisherId, Guid userId)
        {
            IEnumerable<Video>videos = await videoRepository.GetByUserIdWithDetailsAsync(userPublisherId);
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
            IEnumerable<Video> videos = await videoRepository.SearchWithDetailsAsync(search);
            List<VideoDTO> videoDtos = new List<VideoDTO>();
            foreach (Video video in videos)
            {
                VideoDTO videoDto = await CreateVideoDTOFromVideoTypeThinUser(video, userId);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

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
                Interaction? interactionExist = video.Interactions.FirstOrDefault(i => i.UserId == userId && i.VideoId == video.Id);
                if (interactionExist != null)
                {
                    videoDto.LikeStatus = interactionExist.Actionstate;
                }
            }
            return videoDto;
        }

        public async Task<VideoDTO> CreateAsync(VideoDTOCreate videoDTOCreate)
        {
            try
            {
                String videoUrl = await blobProcessingService.UploadFile(videoDTOCreate.VideoFile);
                String posterUrl = await blobProcessingService.UploadFile(videoDTOCreate.PosterFile);

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

                await videoRepository.CreateAsync(videoNew);

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

        public async Task<VideoDTO> UpdateAsync(Guid id, VideoDTOUpdate videoDtoUpdate)
        {
            VideoDTO videoDtoExist = await GetByIdAsync(id, Guid.NewGuid());
            if (videoDtoExist == null)
            {
                return null;
            }
            try
            {
                String videoUrl = videoDtoExist.VideoUrl;
                String posterUrl = videoDtoExist.PosterUrl;

                if (videoDtoUpdate.PosterFile != null)
                {
                    posterUrl = await blobProcessingService.UploadFile(videoDtoUpdate.PosterFile);
                    if (videoDtoExist.PosterUrl != null)
                    {
                        await blobProcessingService.RemoveFile(videoDtoExist.PosterUrl);
                    }
                }

                Video videoToUpdate = new Video();
                videoToUpdate.Id = videoDtoExist.Id;
                videoToUpdate.UserId = videoDtoExist.User.Id;
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
            VideoDTO videoDtoToDelete = await GetByIdAsync(id, Guid.NewGuid());
            if (videoDtoToDelete == null)
            {
                return null;
            }
            Video video = mapper.Map<VideoDTO, Video>(videoDtoToDelete);

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
