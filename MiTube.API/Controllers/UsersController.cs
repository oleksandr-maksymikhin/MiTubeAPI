using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;
using MiTube.DAL.Entities;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly IMapper _mapper;
        //private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync(String userId)
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync()
        {
            //check User authorisation
            //StatusCodeResult checkUserAuthorisation = SessionProcessionHelper(userId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}


            IEnumerable <UserDTO> usersDTO = await _userService.GetAllWithDetailsAsync();
            if (usersDTO == null)
            {
                return NotFound();
            }
            return Ok(usersDTO);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<UserDTO>> GetByIdAsync(String userId, Guid id)
        public async Task<ActionResult<UserDTO>> GetByIdWithDetailsAsync(Guid id)
        {
            //check User authorisation
            //StatusCodeResult checkUserAuthorisation = SessionProcessionHelper(userId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}


            UserDTO userDto = await _userService.GetByIdWithDetailsAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return userDto;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] UserDTOCreateUpdate userDtoCreateUpdate)
        //public async Task<IActionResult> PutAsync(Guid id, [FromForm] UserDTOCreateUpdate userDtoCreateUpdate)
        {
            //check User authorisation
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            //StatusCodeResult checkUserAuthorisation = CheckSession(sessionId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}


            if (id != userDtoCreateUpdate.Id)
            {
                return BadRequest();
            }

            UserDTO updatedUserDto = await _userService.UpdateAsync(id, userDtoCreateUpdate);
            if (updatedUserDto == null)
            {
                return NotFound();
            }

            return Ok(updatedUserDto);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostAsync([FromForm] String sessionId, [FromForm] UserDTOCreateUpdate userDto)
        {
            //check User authorisation
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            //StatusCodeResult checkUserAuthorisation = CheckSession(sessionId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}

            UserDTO createdUserDto = await _userService.CreateAsync(userDto);

            //try to check this return ??? check Id - request produce error BUT entity in DB is created
            //return CreatedAtAction("GetUserByIdAsync", new { id = createdUserDto.Id }, createdUserDto);
            //return CreatedAtAction("GetUser", new { id = userDto.Id }, userDto);

            return createdUserDto;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromBody]String sessionId, Guid id)
        //public async Task<IActionResult> DeleteAsync(Guid id)
        {
            //check User authorisation
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            //StatusCodeResult checkUserAuthorisation = CheckSession(sessionId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}


            UserDTO UserDtoDeleted = await _userService.DeleteAsync(id);
            if (UserDtoDeleted == null)
            {
                return NotFound();
            }
            return Ok(UserDtoDeleted);
        }

        //check user authorisation in session. If userId does not exist in session -> return status code: 401 -> in UI there should be view Authorise
        // !!!!!!!!!!!!!!!! is it correct to return ActionResult? ??????????????
        //private StatusCodeResult CheckSession(String sessionId)
        //{
        //    String? loggedUserId = HttpContext.Session.GetString(SessionVeriables.SessionKeyUserId);
        //    if (loggedUserId?.ToLower() == sessionId.ToLower())
        //    {
        //        return StatusCode(StatusCodes.Status200OK);
        //    }
        //    return StatusCode(StatusCodes.Status401Unauthorized);           //return status code: 401 Unauthorized
        //}

        //private bool UserExists(Guid id)
        //{
        //    return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
