using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.BLL.Interfaces;
using MiTube.BLL.DTO;
using MiTube.BLL.Services;
using MiTube.API.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentsController(ICommentService service)
        {
            _service = service;
        }

        //// GET: api/v1/Comments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComment()
        //{
        //    IEnumerable<CommentDTO> comments = await _service.GetAllAsync();

        //    return comments != null ? Ok(comments) : NotFound();
        //}

        // GET: api/v1/Comments/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetUserComments(Guid userId)
        {
            var comments = await _service.GetByUserIdAsync(userId);

            return !comments.IsNullOrEmpty() ? Ok(comments) : NotFound();
        }

        // GET: api/v1/Comments/videoid/5
        [HttpGet("video/{videoid}")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByVideoId(Guid videoid)
        {
            var comments = await _service.GetByVideoIdAsync(videoid);

            return comments != null ? Ok(comments) : NotFound();
        }

        // PUT: api/v1/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromForm]String sessionId, Guid id, [FromForm]CommentDTO commentDto)
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

            await _service.UpdateAsync(commentDto);

            return NoContent();
        }

        // POST: api/v1/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> PostComment([FromForm] String sessionId, [FromForm]CommentDTO commentDto)
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

            CommentDTO? comment = await _service.CreateAsync(commentDto);

            return comment != null ? Ok(comment) : BadRequest("user or video does not exist");
        }

        // DELETE: api/v1/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromBody]String sessionId, Guid id)
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

            await _service.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CommentExists(Guid id)
        {
            return await _service.GetByIdAsync(id) != null;
        }

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
