using Microsoft.AspNetCore.Mvc;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShowImageController : ControllerBase
    {
        private readonly IShowImageService _showImageService;

        public ShowImageController(IShowImageService showImageService)
        {
            _showImageService = showImageService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ShowImage(String url)
        {
            if (url == null)
            {
                return BadRequest();
            }
            try
            {
                Stream stream = await _showImageService.ShowImageByBlobUrl(url);
                var response = File(stream, "image/jpg");
                return response;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
