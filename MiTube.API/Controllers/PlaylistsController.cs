using Microsoft.AspNetCore.Mvc;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService service)
        {
            _playlistService = service;
        }

        // GET: api/Playlists/videos/5
        [HttpGet("videos/{playlistId}")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideos(Guid playlistId)
        {
            IEnumerable<VideoDTO> playlist = await _playlistService.GetVideosByIdAsync(playlistId);
            return playlist != null ? Ok(playlist) : NotFound();
        }

        // GET: api/Playlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylist(Guid id)
        {
            PlaylistDTO playlist = await _playlistService.GetByIdWithDetailsAsync(id);
            return playlist != null ? Ok(playlist) : NotFound();
        }

        // GET: api/Playlists/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Playlist>> GetByUserId(Guid userId)
        {
            IEnumerable<PlaylistDTO> playlistDtos = await _playlistService.GetByUserIdWithDetailsAsync(userId);
            if (playlistDtos == null)
            {
                return NotFound();
            }
            return Ok(playlistDtos);
        }


        // PUT: api/Playlists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist([FromForm] String sessionId, [FromForm]PlaylistDTOCreate playlistDtoUpdate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            await _playlistService.UpdateAsync(playlistDtoUpdate);
            return NoContent();
        }

        // POST: api/Playlists
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> PostPlaylist([FromForm] String sessionId, [FromForm]PlaylistDTOCreate playlistDtoCreate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            PlaylistDTO? playlistDto = await _playlistService.CreateAsync(playlistDtoCreate);
            return playlistDto != null ? Ok(playlistDto) : BadRequest("user not found");
        }

        // POST: api/Playlists/videos
        [HttpPost("videos/")]
        public async Task<ActionResult> PostVideos([FromForm] String sessionId, Guid playlistId, Guid videoId)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            await _playlistService.AddVideoAsync(playlistId, videoId);
            return NoContent();
        }

        // DELETE: api/Playlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist([FromBody] String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            await _playlistService.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> PlaylistExists(Guid id)
        {
            return await _playlistService.GetByIdAsync(id) != null;
        }
    }
}
