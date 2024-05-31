using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly String _key1;
        private readonly String _key2;
        private readonly String _key3;
        private readonly String _key4;

        //public KeysController(string key1, string key2)
        public KeysController()
        {
            _key1 = "service_enru1xe";
            _key2 = "template_n7knuon";
            _key3 = "9SHnPPgARMCVUVe09";
            _key4 = "mitubestudentproject@gmail.com";
        }

        // GET: api/v1/Videos
        [HttpGet("{sessionId}")]
        public async Task<ActionResult<KeyDTO>> GetAllAsync(String sessionId)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            KeyDTO keys = new KeyDTO()
            { Key_1 = _key1, Key_2 = _key2, Key_3 = _key3, Email = _key4};

            return Ok(keys);
        }

        
}
}
