using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiTube.API.Infrastructure;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;

namespace MiTube.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _interactionService;
        //private readonly ILogger<SubscriptionsController> _logger;

        public InteractionsController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        // GET: api/v1/Interactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InteractionDTO>>> GetAllAsync()
        {
            IEnumerable<InteractionDTO> interactionDTOs = await _interactionService.GetAllAsync();
            //// _interactionService.GetAllAsync() return either IEnumerable<InteractionDTO> or Enumerable.Empty<InteractionDTO>();
            //if (interactionDTOs == null)
            //{
            //    return NotFound();
            //}
            return Ok(interactionDTOs);
        }

        // GET: api/v1/Interactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InteractionDTO>> GetByIdAsync(Guid id)
        {
            InteractionDTO interactionDTO = await _interactionService.GetByIdAsync(id);
            //// _interactionService.GetAllAsync() return either InteractionDTO or new InteractionDTO();
            //if (interactionDTO == null)
            //{
            //    return NotFound();
            //}
            return Ok(interactionDTO);
        }



        // GET: api/v1/Videos/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<InteractionDTO>> GetByUserIdAsync(Guid userId)
        {
            //registered user can get his interactins or any user can get interaction of any user?
            //Guid userIdLoggedInGuid = HttpContext.Session.GetUserId();

            IEnumerable<InteractionDTO>? interactionDtos = await _interactionService.GetByUserIdAsync(userId);
            if (interactionDtos == null)
            {
                return NotFound();
            }

            return Ok(interactionDtos);
        }


        // PUT: api/v1/Interactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] InteractionDTO interactionDtoToUpdate)
        //public async Task<IActionResult> PutAsync(Guid id, InteractionDTO interactionDtoToUpdate)
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
                    //throw;
                }
            }
            return Ok(interactionDtoUpdated);
        }

        // POST: api/v1/Interactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InteractionDTO>> PostAsync([FromForm] String sessionId, [FromForm] InteractionDTO interactionDtoToCreate)
        //public async Task<ActionResult<InteractionDTO>> PostAsync(InteractionDTO interactionDtoToCreate)
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
            //return CreatedAtAction("GetLike", new { id = like.Id }, like);
        }

        // DELETE: api/v1/Interactions/5
        [HttpDelete("{id}")]
        private async Task<IActionResult> DeleteAsync([FromBody] String sessionId, Guid id)
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


        // GET: api/v1/Interactions/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<InteractionDTO>> FindByUserIdAsync(Guid id)
        //{
        //    IEnumerable<InteractionDTO> interactionDTO = await _interactionService.FindInteractionByUserIdAsync(id);
        //    if (interactionDTO == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(interactionDTO);
        //}

        // GET: api/v1/Interactions/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<InteractionDTO>> FindByVideoIdAsync(Guid id)
        //{
        //    IEnumerable<InteractionDTO> interactionDTO = await _interactionService.FindInteractionByVideoIdAsync(id);
        //    if (interactionDTO == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(interactionDTO);
        //}




        private async Task<bool> InteractionExists(Guid id)
        {
            return (await _interactionService.GetByIdAsync(id)) != null;
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
