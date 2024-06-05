using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;

namespace MiTube.BLL.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IMapper mapper;
        private readonly IPlaylistRepository playlistRepository;
        private readonly IUserService userService;
        private readonly IVideoService videoService;
        private readonly IBlobProcessingService blobProcessingService;

        String playlistPosterPublicDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPublicDefault.jpg";
        String playlistPosterPrivateDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateDefault.jpg";
        String playlistPosterPrivateWatchLaterDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateWatchLaterDefault.webp";

        Guid guidDefaultEmpty = Guid.Empty;
        public PlaylistService(IMapper mapper, 
                                IPlaylistRepository playlistRepository, 
                                IUserService userService, 
                                IVideoService videoService, 
                                IBlobProcessingService blobProcessingService)
        {
            this.mapper = mapper;
            this.playlistRepository = playlistRepository;
            this.userService = userService;
            this.videoService = videoService;
            this.blobProcessingService = blobProcessingService;
        }

        public async Task AddVideoAsync(Guid playlistId, Guid videoId)
        {
            Playlist? playlist = await playlistRepository.GetByIdAsync(playlistId);
            if(playlist == null) { return; }
            VideoDTO videoDto = await videoService.GetByIdAsync(videoId, guidDefaultEmpty);
            if (videoDto == null) { return; }
            Video video = mapper.Map<VideoDTO, Video>(videoDto);
            playlistRepository.AddVideo(playlist.Id, video);
        }

        public async Task<PlaylistDTO?> CreateAsync(PlaylistDTOCreate playlistDtoCreate)
        {
            UserDTO? user = await userService.GetByIdAsync(playlistDtoCreate.UserId);
            if (user == null) return null;
            playlistDtoCreate.Id = Guid.NewGuid();
            Playlist playlist = mapper.Map<Playlist>(playlistDtoCreate);
            if (playlistDtoCreate.PosterFile == null && playlistDtoCreate.IsPublic == true)
            {
                playlist.PosterUrl = playlistPosterPublicDefault;
            }
            else if(playlistDtoCreate.PosterFile == null && playlistDtoCreate.IsPublic == false)
            {
                playlist.PosterUrl = playlistPosterPrivateDefault;
            }
            else
            {
                String posterUrl = await blobProcessingService.UploadFile(playlistDtoCreate.PosterFile);
                playlist.PosterUrl = posterUrl;
            }
            await playlistRepository.CreateAsync(playlist);
            return mapper.Map<Playlist, PlaylistDTO>(playlist);
        }

        public async Task DeleteAsync(Guid id)
        {
            PlaylistDTO? playlistDto = await GetByIdAsync(id);
            if (playlistDto != null)
            {
                if (playlistDto.PosterUrl != null)
                {
                    await blobProcessingService.RemoveFile(playlistDto.PosterUrl);
                }
                await playlistRepository.DeleteAsync(mapper.Map<PlaylistDTO, Playlist>(playlistDto));
            }
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAllAsync()
        {
            IEnumerable<Playlist> playlists = await playlistRepository.GetAllAsync();
            return playlists == null ?
                Enumerable.Empty<PlaylistDTO>() :
                mapper.Map< IEnumerable <Playlist>, IEnumerable <PlaylistDTO>>(playlists);
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAllWithDetailsAsync(Guid id)
        {
            IEnumerable<Playlist> playlists = await playlistRepository.GetAllWithDetailsAsync();
            if (playlists == null)
            {
                return Enumerable.Empty<PlaylistDTO>();
            }
            IEnumerable<PlaylistDTO> playlistDtos = CreatePlaylistDTOFromPlaylistTypeCollection(playlists);
            return playlistDtos;
        }

        public async Task<PlaylistDTO?> GetByIdAsync(Guid id)
        {
            Playlist? playlist = await playlistRepository.GetByIdAsync(id);
            return playlist == null ? null : mapper.Map<Playlist, PlaylistDTO>(playlist);
        }

        public async Task<PlaylistDTO?> GetByIdWithDetailsAsync(Guid id)
        {
            Playlist? playlist = await playlistRepository.GetByIdWithEverythingAsync(id);
            if (playlist == null)
            {
                return null;
            }
            PlaylistDTO playlistDto = CreatePlaylistDTOFromPlaylistType(playlist);
            return playlistDto;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetByUserIdWithDetailsAsync(Guid userId)
        {
            IEnumerable<Playlist> playlists = await playlistRepository.GetByUserIdWithDetailsAsync(userId);
            if (playlists == null)
            {
                return Enumerable.Empty<PlaylistDTO>();
            }
            IEnumerable<PlaylistDTO> playlistDtos = CreatePlaylistDTOFromPlaylistTypeCollection(playlists);
            return playlistDtos;
        }

        public async Task<IEnumerable<VideoDTO>> GetVideosByIdAsync(Guid id)
        {
            Playlist? playlist = await playlistRepository.GetByIdWithVideosAsync(id);
            if (playlist == null) return Enumerable.Empty<VideoDTO>();
            IEnumerable<Video> videos = playlist.Videos;
            List<VideoDTO> videoDtos = new List<VideoDTO>();
            foreach (Video video in videos)
            {
                VideoDTO videoDto = await videoService.GetByIdWithDetailsAsync(video.Id, guidDefaultEmpty);
                videoDtos.Add(videoDto);
            }
            return videoDtos;
        }

        public async Task<PlaylistDTO?> UpdateAsync(PlaylistDTOCreate playlistDTOUpdate)
        {
            Playlist? inbound = await playlistRepository.GetByIdWithUserAsync(playlistDTOUpdate.Id);
            Playlist incoming = mapper.Map<Playlist>(playlistDTOUpdate);
            if (playlistDTOUpdate.PosterFile != null)
            {
                await blobProcessingService.RemoveFile(inbound.PosterUrl);
                incoming.PosterUrl = await blobProcessingService.UploadFile(playlistDTOUpdate.PosterFile);
            }
            else
            {
                incoming.PosterUrl = inbound.PosterUrl;
            }
            if (inbound == null || incoming.UserId != inbound.UserId) return null;
            return mapper.Map<PlaylistDTO>(
                    inbound.UserId != incoming.UserId ?
                    inbound :
                    await playlistRepository.UpdateAsync(incoming)
                    );
        }

        private PlaylistDTO CreatePlaylistDTOFromPlaylistType(Playlist playlist)
        {
            PlaylistDTO playlistDTO = mapper.Map<Playlist, PlaylistDTO>(playlist);
            playlistDTO.VideoQuantity = playlist.Videos.Count();
            return playlistDTO;
        }

        private IEnumerable<PlaylistDTO> CreatePlaylistDTOFromPlaylistTypeCollection(IEnumerable<Playlist> playlists)
        {
            List<PlaylistDTO> playlistDtos = new List<PlaylistDTO>();
            foreach (Playlist playlist in playlists)
            {
                PlaylistDTO playlistDto = mapper.Map<Playlist, PlaylistDTO>(playlist);
                playlistDto.VideoQuantity = playlist.Videos.Count();
                playlistDtos.Add(playlistDto);
            }
            return playlistDtos;
        }
    }
}
