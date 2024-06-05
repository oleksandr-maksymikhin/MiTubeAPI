using Microsoft.AspNetCore.Mvc;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync()
        {
            IEnumerable <UserDTO> usersDTO = await _userService.GetAllWithDetailsAsync();
            if (usersDTO == null)
            {
                return NotFound();
            }
            return Ok(usersDTO);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetByIdWithDetailsAsync(Guid id)
        {
            UserDTO userDto = await _userService.GetByIdWithDetailsAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return userDto;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] UserDTOCreateUpdate userDtoCreateUpdate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
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
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostAsync([FromForm] String sessionId, [FromForm] UserDTOCreateUpdate userDto)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            UserDTO createdUserDto = await _userService.CreateAsync(userDto);
            return createdUserDto;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromBody]String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            UserDTO UserDtoDeleted = await _userService.DeleteAsync(id);
            if (UserDtoDeleted == null)
            {
                return NotFound();
            }
            return Ok(UserDtoDeleted);
        }
    }
}
