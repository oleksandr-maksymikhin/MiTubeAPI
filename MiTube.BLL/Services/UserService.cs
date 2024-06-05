using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;
using MiTube.DAL.Entities;

namespace MiTube.BLL.Services
{
    public class UserService: IUserService
    {
        private IMapper mapper;
        private UserRepository userRepository;

        private readonly IBlobProcessingService blobProcessingService;
        private readonly IPlaylistRepository playlistRepository;

        String userPosterDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_userPosterDefault.jpg";
        String userBannerDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_userBannerDefault.jpg";


        String playlistPosterPublicDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPublicDefault.jpg";
        String playlistPosterPrivateDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateDefault.jpg";
        String playlistPosterPrivateWatchLaterDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateWatchLaterDefault.webp";

        String playlistNamePublicDefault = "public playlist";
        String playlistNamePrivateDefault = "private playlist";
        String playlistNamePrivateWatchLaterDefault = "watch later";

        String playlistDescriptionPublicDefault = "this is my public playlist";
        String playlistDescriptionDefault = "this is my private playlist";
        String playlistDescriptionPrivateWatchLaterDefault = "never rush";

        public UserService(IMapper mapper, UserRepository userRepository, BlobProcessingService blobProcessingService, PlaylistRepository playlistRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.blobProcessingService = blobProcessingService;
            this.playlistRepository = playlistRepository;
        }
        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            IEnumerable<User> users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
        }

        public async Task<IEnumerable<UserDTO>> GetAllWithDetailsAsync()
        {
            IEnumerable<User> users = await userRepository.GetAllWithDetailsAsync();
            if (users == null)
            {
                return null;
            }
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (User user in users)
            {
                UserDTO userDTO = await CreateUserDTOFromUserType(user);
                userDTOs.Add(userDTO);
            }
            return userDTOs;
        }

        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            User user = await userRepository.GetByIdAsync(id);
            UserDTO userDtoById = mapper.Map<User, UserDTO>(user);
            return userDtoById;
        }

        public async Task<UserDTO?> GetByIdWithDetailsAsync(Guid id)
        {
            User? user = await userRepository.GetByIdWithDetailsAsync(id);
            if (user == null)
            {
                return null;
            }
            UserDTO userDto = await CreateUserDTOFromUserType(user);
            return userDto;
        }

        private async Task<UserDTO> CreateUserDTOFromUserType(User user)
        {
            UserDTO userDto = mapper.Map<User, UserDTO>(user);
            userDto.UserTypeDescription = user.UserType.Description;
            userDto.Email = user.Usercredentials.Email;
            userDto.IsPremium = false;
            if (user.Subscriptions != null)
            {
                userDto.SubscribersQuantity = user.Subscriptions.Count();
            }
            userDto.VideosQuantity = user.Videos != null ? user.Videos.Count() : 0;
            userDto.CommentsQuantity = user.Comments != null ? user.Comments.Count() : 0;
            userDto.VideoViewQuantity = user.Interactions != null ? user.Interactions.Count() : 0;
            int allVideosDuration = 0;
            if (user.Videos != null)
            {
                foreach (Video video in user.Videos)
                {
                    allVideosDuration += video.Duration;
                }
            }
            userDto.AllVideosDuration = allVideosDuration;
            userDto.WatchLaterPlaylistId = user?.Playlists?.Where(playlist => playlist.Name.Contains("watch later")).FirstOrDefault()?.Id;
            return userDto;
        }

        public async Task<UserDTO> CreateAsync(UserDTOCreateUpdate userDtoCreateUpdate)
        {
            userDtoCreateUpdate.Id = Guid.NewGuid();
            User userToCreate = mapper.Map<UserDTOCreateUpdate, User>(userDtoCreateUpdate);
            userToCreate.PosterUrl = userPosterDefault;
            userToCreate.BanerUrl = userBannerDefault;
            userToCreate.Name = userDtoCreateUpdate.Name == null ? "name_" + DateTime.Now.ToString() : userDtoCreateUpdate.Name;
            userToCreate.Nickname = userDtoCreateUpdate.Nickname == null ? "nickname_" + DateTime.Now.ToString() : userDtoCreateUpdate.Name;
            userToCreate.Description = userDtoCreateUpdate.Description == null ? "" : userDtoCreateUpdate.Name;
            await userRepository.CreateAsync(userToCreate);

            //create 3 default playlists for new user
            Playlist playlistPublicDefault = new Playlist()
            {
                Id = Guid.NewGuid(),
                UserId = userDtoCreateUpdate.Id,
                Name = playlistNamePublicDefault,
                Description = playlistDescriptionPublicDefault,
                PosterUrl = playlistPosterPublicDefault,
                IsPublic = true,
            };
            Playlist playlistPrivateDefault = new Playlist()
            {
                Id = Guid.NewGuid(),
                UserId = userDtoCreateUpdate.Id,
                Name = playlistNamePrivateDefault,
                Description = playlistDescriptionDefault,
                PosterUrl = playlistPosterPrivateDefault,
                IsPublic = false,
            };
            Playlist playlistPrivateWatchLaterDefault = new Playlist()
            {
                Id = Guid.NewGuid(),
                UserId = userDtoCreateUpdate.Id,
                Name = playlistNamePrivateWatchLaterDefault,
                Description = playlistDescriptionPrivateWatchLaterDefault,
                PosterUrl = playlistPosterPrivateWatchLaterDefault,
                IsPublic = false,
            };
            await playlistRepository.CreateAsync(playlistPublicDefault);
            await playlistRepository.CreateAsync(playlistPrivateDefault);
            await playlistRepository.CreateAsync(playlistPrivateWatchLaterDefault);

            UserDTO userDtoToCreate = mapper.Map<User, UserDTO>(userToCreate);
            return userDtoToCreate;
        }

        public async Task<UserDTO> UpdateAsync(Guid id, UserDTOCreateUpdate userDtoUpdate)
        {
            UserDTO userDtoExist = await GetByIdAsync(id);
            if (userDtoExist == null)
            {
                return null;
            }
            try
            {
                String posterUrl = userDtoExist.PosterUrl;
                String banerUrl = userDtoExist.BanerUrl;

                String posterUrlToDelete = userDtoExist.PosterUrl;
                String banerUrlToDelete = userDtoExist.BanerUrl;

                if (userDtoUpdate.PosterFile != null)
                {
                    posterUrl = await blobProcessingService.UploadFile(userDtoUpdate.PosterFile);
                }
                if (userDtoUpdate.BanerFile != null)
                {
                    banerUrl = await blobProcessingService.UploadFile(userDtoUpdate.BanerFile);
                }

                User userToUpdate = mapper.Map<UserDTOCreateUpdate, User>(userDtoUpdate);
                userToUpdate.PosterUrl = posterUrl;
                userToUpdate.BanerUrl = banerUrl;
                userToUpdate.Name = userDtoUpdate.Name == null ? userDtoExist.Name : userDtoUpdate.Name;
                userToUpdate.Nickname = userDtoUpdate.Nickname == null ? userDtoExist.Nickname : userDtoUpdate.Nickname;
                userToUpdate.Description = userDtoUpdate.Description == null ? userDtoExist.Description : userDtoUpdate.Description;
                User userUpdated = await userRepository.UpdateAsync(userToUpdate);

                //moved delete Azur.Blob after saving changes in SQLServer
                if (userDtoUpdate.PosterFile != null && userDtoExist.PosterUrl != null && userDtoExist.PosterUrl != userPosterDefault)
                {
                    await blobProcessingService.RemoveFile(posterUrlToDelete);
                }
                if (userDtoUpdate.BanerFile != null && userDtoExist.BanerUrl != null && userDtoExist.BanerUrl != userBannerDefault)
                {
                    await blobProcessingService.RemoveFile(banerUrlToDelete);
                }
                User? userWithDetailsUpdated = await userRepository.GetByIdWithDetailsAsync(id);
                UserDTO userDtoUpdated = await CreateUserDTOFromUserType(userWithDetailsUpdated);
                return userDtoUpdated;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<UserDTO> DeleteAsync(Guid id)
        {
            UserDTO userDtoToDelete = await GetByIdAsync(id);
            if (userDtoToDelete == null)
            {
                return null;
            }
            User user = mapper.Map<UserDTO, User>(userDtoToDelete);
            await userRepository.DeleteAsync(user);
            if (user.PosterUrl != null && userDtoToDelete.PosterUrl != userPosterDefault)
            {
                await blobProcessingService.RemoveFile(user.PosterUrl);
            }
            if (user.BanerUrl != null && userDtoToDelete.BanerUrl != userBannerDefault)
            {
                await blobProcessingService.RemoveFile(user.BanerUrl);
            }
            return userDtoToDelete;
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<UserDTO>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }

    }
}
