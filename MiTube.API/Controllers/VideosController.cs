using Microsoft.AspNetCore.Mvc;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        // GET: api/v1/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetAllAsync()
        {
            Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();
            IEnumerable <VideoDTO> videoDtos = await _videoService.GetAllWithDetailsAsync(userIdLoggedInGuid);
            if (videoDtos == null)
            {
                return NotFound();
            }
            return Ok(videoDtos);
        }

        // GET: api/v1/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDTO>> GetByIdAsync(Guid id)
        {
            Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();
            VideoDTO videoDto = await _videoService.GetByIdWithDetailsAsync(id, userIdLoggedInGuid);
            if (videoDto == null)
            {
                return NotFound();
            }
            return Ok(videoDto);
        }

        // GET: api/v1/Videos/user/5
        [HttpGet("user/{userPublisherId}")]
        public async Task<ActionResult<VideoDTO>> GetByUserIdAsync(Guid userPublisherId)
        {
            Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();
            IEnumerable <VideoDTO> videoDtos = await _videoService.GetByUserIdWithDetailsAsync(userPublisherId, userIdLoggedInGuid);
            if (videoDtos == null)
            {
                return NotFound();
            }
            return Ok(videoDtos);
        }

        // GET: api/v1/Videos/5
        [HttpGet("search/{search}")]
        public async Task<ActionResult<VideoDTO>> SearchVideoAsync([FromRoute]String search)
        {
            Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();
            IEnumerable<VideoDTO> videoDtos = await _videoService.SearchVideoWithDetailsAsync(search, userIdLoggedInGuid);
            if (videoDtos == null)
            {
                return NotFound();
            }
            return Ok(videoDtos);
        }

        // GET: api/v1/Videos/5
        [HttpGet("search")]
        public async Task<ActionResult<VideoDTO>> SearchVideoAsync()
        {
            Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();
            IEnumerable<VideoDTO> videoDtos = await _videoService.GetAllWithDetailsAsync(userIdLoggedInGuid);
            if (videoDtos == null)
            {
                return NotFound();
            }
            return Ok(videoDtos);
        }

        // PUT: api/v1/Videos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] VideoDTOUpdate videoDtoUpdate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (id != videoDtoUpdate.Id)
            {
                return BadRequest();
            }
            VideoDTO videoDtoUpdated = await _videoService.UpdateAsync(id, videoDtoUpdate);
            return Ok(videoDtoUpdated);
        }

        // POST: api/v1/Videos
        [HttpPost]
        public async Task<ActionResult<VideoDTO>> PostAsync([FromForm] String sessionId, [FromForm] VideoDTOCreate videoDtoCreate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            VideoDTO createdVideoDto = await _videoService.CreateAsync(videoDtoCreate);
            return createdVideoDto;
        }

        // DELETE: api/v1/Videos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromBody]String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            VideoDTO VideoDtoDeleted = await _videoService.DeleteAsync(id);
            if (VideoDtoDeleted == null)
            {
                return NotFound();
            }
            return Ok(VideoDtoDeleted);
        }
    }
}
