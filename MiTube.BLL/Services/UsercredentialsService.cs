using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MiTube.BLL.DTO;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using MiTube.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Services
{
    public class UsercredentialsService : IUsercredentialsService
    {
        private readonly IMapper mapper;
        //
        private readonly IUserService userService;
        private readonly UsercredentialsRepository usercredentialsRepository;

        //public const string SessionKeyName = "_Name";
        //HttpContext httpContext;

        //public UsercredentialsService(IMapper mapper, UsercredentialsRepository usercredentialsRepository)
        //public UsercredentialsService(IMapper mapper, UserService userService, UsercredentialsRepository usercredentialsRepository, HttpContext httpContext)
        public UsercredentialsService(IMapper mapper, UserService userService, UsercredentialsRepository usercredentialsRepository)
        {
            this.mapper = mapper;
            //
            this.userService = userService;
            this.usercredentialsRepository = usercredentialsRepository;
            //this.httpContext = httpContext;
        }

        public async Task<UsercredentialsDTO> CreateAsync(UsercredentialsDTO usercredentialsDto)
        {
            Usercredentials? usercredentialsExist = await usercredentialsRepository.SearchByEmailAsync(usercredentialsDto.Email);
            if (usercredentialsExist != null)
            {
                return null;
            }


            //create User to get UserID - foreign key for Usercredentials
            //
            UserDTOCreateUpdate newUserDto = new UserDTOCreateUpdate();
            newUserDto.UserTypeId = (int)UserTypeEnum.registered;
            UserDTO createdUserDto = await userService.CreateAsync(newUserDto);


            usercredentialsDto.Id = Guid.NewGuid();
            //
            usercredentialsDto.UserId = createdUserDto.Id;
            Usercredentials newUserCredentials = mapper.Map<UsercredentialsDTO, Usercredentials>(usercredentialsDto);

            ////when using password hash
            //newUserCredentials.Password = GetHash(usercredentialsDto.Password);

            await usercredentialsRepository.CreateAsync(newUserCredentials);
            return usercredentialsDto;
        }

        public async Task<UsercredentialsDTO> DeleteAsync(Guid id)
        {
            UsercredentialsDTO usercredentialsDtoToDelete = await GetByIdAsync(id);
            if (usercredentialsDtoToDelete == null)
            {
                return null;
            }
            Usercredentials usercredentialsToDelete = mapper.Map<UsercredentialsDTO, Usercredentials>(usercredentialsDtoToDelete);
            await usercredentialsRepository.DeleteAsync(usercredentialsToDelete);
            return usercredentialsDtoToDelete;
        }

        public async Task<IEnumerable<UsercredentialsDTO>> GetAllAsync()
        {
            IEnumerable<Usercredentials> usercredentials = await usercredentialsRepository.GetAllAsync();
            IEnumerable<UsercredentialsDTO> usercredentialsDTO = mapper.Map<IEnumerable<Usercredentials>, IEnumerable<UsercredentialsDTO>>(usercredentials);
            return usercredentialsDTO;
        }

        public async Task<UsercredentialsDTO> GetByIdAsync(Guid id)
        {
            Usercredentials usercredentials = await usercredentialsRepository.GetByIdAsync(id);
            UsercredentialsDTO usercredentialsDto = mapper.Map<Usercredentials, UsercredentialsDTO>(usercredentials);
            return (usercredentialsDto);
        }


        public async Task<UsercredentialsDTO> UpdateAsync(Guid id, UsercredentialsDTO usercredentialsDto)
        {
            //UsercredentialsDTO usercredentialsDtoToUpdate = await GetByIdAsync(id);
            Usercredentials usercredentialsToUpdate = mapper.Map<UsercredentialsDTO, Usercredentials>(usercredentialsDto);

            ////when using password hash
            //usercredentialsToUpdate.Password = GetHash(usercredentialsDto.Password);

            Usercredentials usercredentialsUpdated = await usercredentialsRepository.UpdateAsync(usercredentialsToUpdate);
            UsercredentialsDTO usercredentialsDtoUpdated = mapper.Map<Usercredentials, UsercredentialsDTO>(usercredentialsUpdated);
            return usercredentialsDtoUpdated;
        }


        public async Task<UserDTO> Login(UsercredentialsDTO usercredentialsDto)
        {
            UsercredentialsDTO usercredentialsDtoExist = await FindCredentials(usercredentialsDto);
            if (usercredentialsDtoExist == null)
            {
                return null;
            }

            UserDTO UserDtoExist = await userService.GetByIdWithDetailsAsync(usercredentialsDtoExist.UserId);
            if (UserDtoExist == null)
            {
                return null;
            }


            //String userId = UserDtoExist.Id.ToString();
            //HttpContext.
            //HttpContext.Session.SetString(SessionKeyName, userId);




            return UserDtoExist;
            //throw new NotImplementedException();
        }

        public async Task<UserDTO> Logout(string loggedUserIdString)
        {
            Guid loggedUserIdGuid = Guid.Parse(loggedUserIdString);
            UserDTO UserDtoExist = await userService.GetByIdAsync(loggedUserIdGuid);
            if (UserDtoExist == null)
            {
                return null;
            }
            return UserDtoExist;

            //HttpContext.Session.Remove("_Name");                      //logout and remove session by key
            //throw new NotImplementedException();
        }


        public async Task<UsercredentialsDTO> FindCredentials(UsercredentialsDTO usercredentialsDtoToLogin)
        {
            //it should not work because mapper will not be able to MAP null value
            //Usercredentials usercredentialsToFind = mapper.Map<UsercredentialsDTO, Usercredentials>(usercredentialsDtoToLogin);

            Usercredentials usercredentialsToFind = new Usercredentials();


            usercredentialsToFind.Email = usercredentialsDtoToLogin.Email;
            usercredentialsToFind.Password = usercredentialsDtoToLogin.Password;
            
            ////////when using password hash
            //usercredentialsToFind.Password = GetHash(usercredentialsDtoToLogin.Password);

            Usercredentials usercredentialsExist = await usercredentialsRepository.SearchEntityAsync(usercredentialsToFind);
            if (usercredentialsExist == null)
            {
                return null;
            }
            UsercredentialsDTO usercredentialsDtoExist = mapper.Map<Usercredentials, UsercredentialsDTO>(usercredentialsExist);
            return usercredentialsDtoExist;
        }


        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

    }
}
