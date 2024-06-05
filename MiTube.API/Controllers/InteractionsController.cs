using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionsController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        // GET: api/v1/Interactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InteractionDTO>>> GetAllAsync()
        {
            IEnumerable<InteractionDTO> interactionDTOs = await _interactionService.GetAllAsync();
            return Ok(interactionDTOs);
        }

        // GET: api/v1/Interactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InteractionDTO>> GetByIdAsync(Guid id)
        {
            InteractionDTO interactionDTO = await _interactionService.GetByIdAsync(id);
            return Ok(interactionDTO);
        }



        // GET: api/v1/Videos/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<InteractionDTO>> GetByUserIdAsync(Guid userId)
        {
            IEnumerable<InteractionDTO>? interactionDtos = await _interactionService.GetByUserIdAsync(userId);
            if (interactionDtos == null)
            {
                return NotFound();
            }
            return Ok(interactionDtos);
        }


        // PUT: api/v1/Interactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] InteractionDTO interactionDtoToUpdate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (id != interactionDtoToUpdate.Id)
            {
                return BadRequest();
            }
            InteractionDTO interactionDtoUpdated;
            try
            {
                interactionDtoUpdated = await _interactionService.UpdateAsync(id, interactionDtoToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InteractionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok(interactionDtoUpdated);
        }

        // POST: api/v1/Interactions
        [HttpPost]
        public async Task<ActionResult<InteractionDTO>> PostAsync([FromForm] String sessionId, [FromForm] InteractionDTO interactionDtoToCreate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            InteractionDTO interactionDtoCreated;
            try
            {
                interactionDtoCreated = await _interactionService.CreateAsync(interactionDtoToCreate);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok(interactionDtoCreated);
        }

        // DELETE: api/v1/Interactions/5
        [HttpDelete("{id}")]
        private async Task<IActionResult> DeleteAsync([FromBody] String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            try
            {
                await _interactionService.DeleteAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InteractionExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private async Task<bool> InteractionExists(Guid id)
        {
            return (await _interactionService.GetByIdAsync(id)) != null;
        }
    }
}
