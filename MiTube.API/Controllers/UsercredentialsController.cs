using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;
using MiTube.API.Infrastructure;
using MiTube.DAL.Entities;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsercredentialsController : ControllerBase
    {
        //private readonly IMapper _mapper;
        //private readonly ILogger<UsercredentialsController> logger ;
        private readonly IUsercredentialsService _userCredentialsService;

        //public const string SessionKeyUserId = "LoggedUserId";
        //public const string SessionKeySessionId = "LoggedSessionId";

        public UsercredentialsController(UsercredentialsService userCredentialsService)
        {
            _userCredentialsService = userCredentialsService;
        }


        // POST: api/v1/Usercredentials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        //public async Task<ActionResult<IEnumerable<UsercredentialsDTO>>> GetAllAsync()
        public async Task<ActionResult<IEnumerable<UsercredentialsDTO>>> GetAllAsync(String sessionId)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            //check User authorisation
            //StatusCodeResult checkUserAuthorisation = CheckSession(sessionId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status401Unauthorized)
            //{
            //    return checkUserAuthorisation;
            //}

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
        //public async Task<ActionResult<UsercredentialsDTO>> GetByIdAsync(Guid id)
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

            UsercredentialsDTO usercredentialsDto = await _userCredentialsService.GetByIdAsync(id);
            if (usercredentialsDto == null)
            {
                return NotFound();
            }
            return Ok(usercredentialsDto);
        }

        // PUT: api/v1/Usercredentials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] UsercredentialsDTO usercredentialsDto)
        //public async Task<IActionResult> PutAsync(Guid id, UsercredentialsDTO usercredentialsDto)
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
            //Put UserID in session
            //HttpContext.Session.SetString(SessionKeyUserId, vrifiedUserId);
            HttpContext.Session.SetString(SessionProcessionHelper.SessionKeySessionId, vrifiedUserId);
            HttpContext.Session.SetString(SessionProcessionHelper.SessionKeyUserId, vrifiedUserId);

            return Ok(userLogin);
        }

        // GET: api/v1/Usercredentials/Logout
        //[HttpGet("Logout/{sessionId}")]
        [HttpPost("Logout")]
        //public async Task<ActionResult<UserDTO>> Logout(String sessionId)
        public async Task<ActionResult<UserDTO>> Logout([FromForm] String sessionId)
        {
            //check User authorisation
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
                return StatusCode(StatusCodes.Status401Unauthorized);           //return status code: 401 Unauthorized
            }
            HttpContext.Session.Remove(SessionProcessionHelper.SessionKeySessionId);
            HttpContext.Session.Remove(SessionProcessionHelper.SessionKeyUserId);
            return Ok(userLogout);
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

    }
}
