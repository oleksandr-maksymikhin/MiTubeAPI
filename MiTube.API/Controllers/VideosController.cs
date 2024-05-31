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
    public class VideosController : ControllerBase
    {
        //private readonly ILogger<UsersController> _logger;
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] VideoDTOUpdate videoDtoUpdate)
        //public async Task<IActionResult> PutAsync(Guid id, [FromForm] VideoDTOCreate videoDtoUpdate)
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

            if (id != videoDtoUpdate.Id)
            {
                return BadRequest();
            }

            VideoDTO videoDtoUpdated = await _videoService.UpdateAsync(id, videoDtoUpdate);

            return Ok(videoDtoUpdated);
        }

        // POST: api/v1/Videos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoDTO>> PostAsync([FromForm] String sessionId, [FromForm] VideoDTOCreate videoDtoCreate)
        //public async Task<ActionResult<VideoDTO>> PostAsync([FromForm] VideoDTOCreate videoDtoCreate)
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

            VideoDTO createdVideoDto = await _videoService.CreateAsync(videoDtoCreate);

            //!!!!!!!!!!!!!!there should be some check if video was not saved

            return createdVideoDto;
        }

        // DELETE: api/v1/Videos/5
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

            VideoDTO VideoDtoDeleted = await _videoService.DeleteAsync(id);
            if (VideoDtoDeleted == null)
            {
                return NotFound();
            }
            return Ok(VideoDtoDeleted);
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

        //    private bool VideoExists(Guid id)
        //    {
        //        return (_context.Video?.Any(e => e.Id == id)).GetValueOrDefault();
        //    }
    }
}
