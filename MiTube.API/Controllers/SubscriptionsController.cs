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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
 
        public SubscriptionsController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET: api/v1/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDTO>>> GetAllAsync()
        {
            IEnumerable<SubscriptionDTO> subscriptionDtos = await _subscriptionService.GetAllAsync();
            if (subscriptionDtos == null)
            {
                return NotFound();
            }
            return Ok(subscriptionDtos);
        }

        // GET: api/v1/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetByIdAsync(Guid id)
        {
            SubscriptionDTO? subscriptionDto = await _subscriptionService.GetByIdAsync(id);
            if (subscriptionDto == null)
            {
                return NotFound();
            }
            return Ok(subscriptionDto);
        }

        // GET: api/v1/Subscriptions/5
        [HttpGet("{publisherId, subscriberId}")]
        public async Task<ActionResult<bool>> GetByPublisherIdSubscriberIdAsync(Guid publisherId, Guid subscriberId)
        {
            SubscriptionDTO? subscriptionDto = await _subscriptionService.GetByPublisherIdSubscriberIdAsync(publisherId, subscriberId);
            if (subscriptionDto != null)
            {
                return NotFound();
            }
            return Ok(false);
        }

        // GET: api/v1/Subscriptions/subscribers/5
        [HttpGet("subscribers/{publisherId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetSubscribersByPublisherIdAsync(Guid publisherId)
        {
            IEnumerable<UserDTO>? userDtoSubscribers = await _subscriptionService.GetSubscribersByPublisherIdAsync(publisherId);
            if (userDtoSubscribers == null)
            {
                return NotFound();
            }
            return Ok(userDtoSubscribers);
        }

        // GET: api/v1/Subscriptions/publishers/5
        [HttpGet("publishers/{subscriberId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetPublishersBySubscriberIdAsync(Guid subscriberId)
        {
            IEnumerable<UserDTO>? userDtoPublishers = await _subscriptionService.GetPublishersBySubscriberIdAsync(subscriberId);
            if (userDtoPublishers == null)
            {
                return NotFound();
            }
            return Ok(userDtoPublishers);
        }

        // PUT: api/v1/Subscriptions/5
        [HttpPut("{id}")]
        private async Task<IActionResult> PutAsync([FromForm] String sessionId, Guid id, [FromForm] SubscriptionDTO subscriptionDtoToUpdate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (id != subscriptionDtoToUpdate.Id)
            {
                return BadRequest();
            }
            SubscriptionDTO? SubscriptionDtoUpdated;
            try
            {
                SubscriptionDtoUpdated = await _subscriptionService.UpdateAsync(id, subscriptionDtoToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest("Server error in Subscription Update");
                }
            }
            if (SubscriptionDtoUpdated == null)
            {
                return BadRequest("Subscription was not found");
            }

            return Ok(SubscriptionDtoUpdated);
        }

        // POST: api/v1/Subscriptions
        [HttpPost]
        public async Task<ActionResult<SubscriptionDTO>> PostAsync([FromForm] String sessionId, [FromForm] SubscriptionDTO subscriptionDtoToCreate)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            SubscriptionDTO? subscriptionDtoCreated;
            try
            {
                subscriptionDtoCreated = await _subscriptionService.CreateAsync(subscriptionDtoToCreate);
            }
            catch (Exception)
            {
                throw;
            }
            if (subscriptionDtoCreated == null)
            {
                return BadRequest("Publisher or Subscriber does not exist");
            }
            return Ok(subscriptionDtoCreated);
        }

        // DELETE: api/v1/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromBody] String sessionId, Guid id)
        {
            bool sessionIdLogedIn = HttpContext.Session.CheckSessionId(sessionId);
            if (!sessionIdLogedIn)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            try
            {
                await _subscriptionService.DeleteAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubscriptionExists(id))
                {
                    return NotFound();
                }
                return NoContent();
            }

            return NoContent();
        }

        private async Task<bool> SubscriptionExists(Guid id)
        {
            return (await _subscriptionService.GetByIdAsync(id)) != null;
        }
    }
}
