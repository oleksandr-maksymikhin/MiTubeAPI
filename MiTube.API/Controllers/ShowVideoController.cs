using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShowVideoController : ControllerBase
    {
        private readonly IShowVideoService _showVideoService;
        //private readonly IInteractionService _interactionService;

        //public ShowVideoController(IShowVideoService showVideoService, IInteractionService interactionService)
        public ShowVideoController(IShowVideoService showVideoService)
        {
            _showVideoService = showVideoService;
            //_interactionService = interactionService;
        }

        //[HttpGet("DownloadFile")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status206PartialContent, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult> ShowVideo(String url, Guid userId, Guid videoId, String? sessionId)
        public async Task<ActionResult> ShowVideo(String url)
        {
            if (url == null)
            {
                return BadRequest();
            }

            //StatusCodeResult checkUserAuthorisation = SessionProcessionHelper(sessionId);
            //if (checkUserAuthorisation.StatusCode == StatusCodes.Status200OK)
            //{
            //    if (userId != Guid.Empty && videoId != Guid.Empty)
            //    {
            //        await _interactionService.CreateInteractionViewAsync(userId, videoId);
            //    }
            //}

            try
            {
                Stream stream = await _showVideoService.ShowVideoByBlobUrl(url);
                var response = File(stream, "video/mp4", enableRangeProcessing: true);
                return response;
            }
            catch (Exception)
            {
                //throw;
                return StatusCode(500, "Internal Server Error");
            }
        }


        //private StatusCodeResult SessionProcessionHelper(String sessionId)
        //{
        //    String? loggedUserId = HttpContext.Session.GetString(SessionVeriables.SessionKeyUserId);
        //    if (loggedUserId != null && sessionId != null &&  loggedUserId?.ToLower() == sessionId?.ToLower())
        //    {
        //        return StatusCode(StatusCodes.Status200OK);
        //    }
        //    return StatusCode(StatusCodes.Status401Unauthorized);           //return status code: 401 Unauthorized
        //}



    }
}
