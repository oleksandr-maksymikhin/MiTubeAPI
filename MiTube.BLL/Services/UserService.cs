using AutoMapper;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;
using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MiTube.BLL.Infrastructure;

namespace MiTube.BLL.Services
{
    public class UserService: IUserService
    {
        private IMapper mapper;
        private UserRepository userRepository;
        //private UsercredentialsRepository userpasswordRepository;

        //private GenericRepositoryIdGuid<User> userRepository;

        //BlobServiceClient blobServiceClient;
        //string blobContainerName = "mitube";

        private readonly IBlobProcessingService blobProcessingService;
        //private readonly IPlaylistService playlistService;
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


        //public UserService(IMapper mapper, UserRepository userRepository, BlobServiceClient blobServiceClient)
        //public UserService(IMapper mapper, UserRepository userRepository, BlobProcessingService blobProcessingService, PlaylistService playlistService)
        //public UserService(IMapper mapper, UserRepository userRepository, BlobProcessingService blobProcessingService)
        public UserService(IMapper mapper, UserRepository userRepository, BlobProcessingService blobProcessingService, PlaylistRepository playlistRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            //this.blobServiceClient = blobServiceClient;
            this.blobProcessingService = blobProcessingService;
            //this.playlistService = playlistService;
            this.playlistRepository = playlistRepository;
            //this.userpasswordRepository = userpasswordRepository;
            //mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>().ReverseMap()).CreateMapper();
            //mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>().ReverseMap()).CreateMapper();
            //mapperCreateUser = new MapperConfiguration(cfg => cfg.CreateMap<UserDTOCreateUpdate, User>().ReverseMap()).CreateMapper();
        }


        //public IEnumerable<UserDTO> GetAllUsers()
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
            //correct after PremiumUser realisation
            userDto.IsPremium = false;
            //correct after Subscription realisation
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
            //userDto.WatchLaterPlaylistId = await playlistService.GetWatchLaterPlaylistIdAsync(user.Id);
            userDto.WatchLaterPlaylistId = user?.Playlists?.Where(playlist => playlist.Name.Contains("watch later")).FirstOrDefault()?.Id;

            return userDto;
        }


            

        //this method will be used only whn we create Usercredentials => User have only UserType, all the rest properties = null.
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
            //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = await blobContainerClient.ExistsAsync();
            //if (!isExist)
            //{
            //    blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //}

            // !!!!!!!!!!!!!!! there is no check if we have to delete previous video/poster from Azure.Blob and reupload a new one !!!!!!!!!!!!!!
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

                //if (userDtoCreateUpdate.PosterFile != null && posterUrl != null)
                //{
                //    await RemoveFile(posterUrl, blobContainerClient);
                //    posterUrl = await UploadFile(userDtoCreateUpdate.PosterFile, blobContainerClient);
                //}
                //if (userDtoCreateUpdate.BanerFile != null && banerUrl != null)
                //{
                //    await RemoveFile(banerUrl, blobContainerClient);
                //    banerUrl = await UploadFile(userDtoCreateUpdate.BanerFile, blobContainerClient);
                //}

                //if (userDtoUpdate.PosterFile != null && posterUrl != null && posterUrl != userPosterDefault)
                //{
                //    await blobProcessingService.RemoveFile(posterUrl);
                //}
                //if (userDtoUpdate.BanerFile != null && banerUrl != null && banerUrl != userBannerDefault)
                //{
                //    await blobProcessingService.RemoveFile(banerUrl);
                //}


                if (userDtoUpdate.PosterFile != null)
                {
                    posterUrl = await blobProcessingService.UploadFile(userDtoUpdate.PosterFile);
                }
                if (userDtoUpdate.BanerFile != null)
                {
                    banerUrl = await blobProcessingService.UploadFile(userDtoUpdate.BanerFile);
                }

                User userToUpdate = mapper.Map<UserDTOCreateUpdate, User>(userDtoUpdate);
                //userToUpdate.PosterUrl = posterUrl != null ? posterUrl : userDtoExist.PosterUrl;
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

                //UserDTO userDtoUpdated = mapper.Map<User, UserDTO>(userUpdated);
                User? userWithDetailsUpdated = await userRepository.GetByIdWithDetailsAsync(id);
                UserDTO userDtoUpdated = await CreateUserDTOFromUserType(userWithDetailsUpdated);

                return userDtoUpdated;
            }
            catch (Exception)
            {
                throw;                                                              //!!!!!!!!!!!!!consider exception processing
            }

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

        public async Task<UserDTO> DeleteAsync(Guid id)
        {
            //error is possible here !!!!! object lock os some other? I don't remember the case
            UserDTO userDtoToDelete = await GetByIdAsync(id);
            if (userDtoToDelete == null)
            {
                return null;
            }
            User user = mapper.Map<UserDTO, User>(userDtoToDelete);

            //try to make dispose of userDtoToDelete
            //userDtoToDelete = null;

            // !!!!!!! problem !!!!!
            //System.InvalidOperationException: The instance of entity type 'User' cannot be tracked because
            //another instance with the same key value for { 'Id'} is already being tracked.
            //When attaching existing entities, ensure that only one entity instance with a given key value is attached.
            //Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.

            //access to BLOB -> but it should be moved somevare in DI
            //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = await blobContainerClient.ExistsAsync();
            //if (!isExist)
            //{
            //    blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(blobContainerName);
            //}

            await userRepository.DeleteAsync(user);

            if (user.PosterUrl != null && userDtoToDelete.PosterUrl != userPosterDefault)
            {
                await blobProcessingService.RemoveFile(user.PosterUrl);
            }
            if (user.BanerUrl != null && userDtoToDelete.BanerUrl != userBannerDefault)
            {
                await blobProcessingService.RemoveFile(user.BanerUrl);
            }


            //await userRepository.DeleteAsync(user);
            return userDtoToDelete;
        }

        // does not delete files of Poster and banner in Blob
        //private async Task RemoveFile(string url, BlobContainerClient blobContainerClient)
        //{
        //    //string blobName = fileToUpload.FileName;
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
        public Task<IEnumerable<UserDTO>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }

    }
}
