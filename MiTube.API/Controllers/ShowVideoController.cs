using Microsoft.AspNetCore.Mvc;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShowVideoController : ControllerBase
    {
        private readonly IShowVideoService _showVideoService;
        public ShowVideoController(IShowVideoService showVideoService)
        {
            _showVideoService = showVideoService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status206PartialContent, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ShowVideo(String url)
        {
            if (url == null)
            {
                return BadRequest();
            }
            try
            {
                Stream stream = await _showVideoService.ShowVideoByBlobUrl(url);
                var response = File(stream, "video/mp4", enableRangeProcessing: true);
                return response;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
