using Microsoft.AspNetCore.Mvc;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;
using MiTube.API.Infrastructure;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsercredentialsController : ControllerBase
    {
        private readonly IUsercredentialsService _userCredentialsService;

        public UsercredentialsController(UsercredentialsService userCredentialsService)
        {
            _userCredentialsService = userCredentialsService;
        }

        // POST: api/v1/Usercredentials
        [HttpPost]
        public async Task<ActionResult<UsercredentialsDTO>> PostAsync(UsercredentialsDTO usercredentialsDto)
        {
            UsercredentialsDTO createdUsercredentialsDto = await _userCredentialsService.CreateAsync(usercredentialsDto);
            if (createdUsercredentialsDto == null)
            {
                return BadRequest("this email already registered");
            }
            return createdUsercredentialsDto;
        }


        // GET: api/v1/Usercredentials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsercredentialsDTO>>> GetAllAsync(String sessionId)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            IEnumerable<UsercredentialsDTO> Usercredentials = await _userCredentialsService.GetAllAsync();
            if (Usercredentials == null)
            {
                return NotFound();
            }
            return Ok(Usercredentials);
        }

        // GET: api/v1/Usercredentials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsercredentialsDTO>> GetByIdAsync(String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            UsercredentialsDTO usercredentialsDto = await _userCredentialsService.GetByIdAsync(id);
            if (usercredentialsDto == null)
            {
                return NotFound();
            }
            return Ok(usercredentialsDto);
        }

        // PUT: api/v1/Usercredentials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] UsercredentialsDTO usercredentialsDto)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (id != usercredentialsDto.Id)
            {
                return BadRequest();
            }
            UsercredentialsDTO updatedUsercredentialsDto = await _userCredentialsService.UpdateAsync(id, usercredentialsDto);
            return Ok(updatedUsercredentialsDto);
        }


        // DELETE: api/v1/Usercredentials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromBody] String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            UsercredentialsDTO deletedUsercredentialsDto = await _userCredentialsService.DeleteAsync(id);
            if (deletedUsercredentialsDto == null)
            {
                return NotFound();
            }
            return Ok(deletedUsercredentialsDto);
        }


        // POST: api/v1/Usercredentials/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(UsercredentialsDTO usercredentialsDtoToLogin)
        {
            UserDTO userLogin = await _userCredentialsService.Login(usercredentialsDtoToLogin);
            if (userLogin == null)
            {
                return NotFound();
            }

            String vrifiedUserId = userLogin.Id.ToString();
            HttpContext.Session.SetString(SessionProcessionHelper.SessionKeySessionId, vrifiedUserId);
            HttpContext.Session.SetString(SessionProcessionHelper.SessionKeyUserId, vrifiedUserId);

            return Ok(userLogin);
        }

        // GET: api/v1/Usercredentials/Logout
        [HttpPost("Logout")]
        public async Task<ActionResult<UserDTO>> Logout([FromForm] String sessionId)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            String? loggedUserId = HttpContext.Session.GetString(SessionProcessionHelper.SessionKeyUserId);
            UserDTO? userLogout = null;
            if (loggedUserId != null)
            {
                userLogout = await _userCredentialsService.Logout(loggedUserId);
            }
            if (userLogout == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            HttpContext.Session.Remove(SessionProcessionHelper.SessionKeySessionId);
            HttpContext.Session.Remove(SessionProcessionHelper.SessionKeyUserId);
            return Ok(userLogout);
        }
    }
}
