
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // body json default
    public class PollsController (IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id , CancellationToken cancellationToken)
        {
            var element = await _pollService.GetAsync(id , cancellationToken);
            return element is null ? NotFound() : Ok(element.Adapt<PollResponse>());
        }
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest poll , CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(poll.Adapt<Poll>());
            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id , [FromBody] PollRequest poll , CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id , poll.Adapt<Poll>() , cancellationToken);
            if(!isUpdated) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id , CancellationToken cancellationToken)
        {
            var isDeleted = await _pollService.DeleteAsync(id , cancellationToken);
            if(!isDeleted) return NotFound();
            return NoContent();
        }
        [HttpPut("{id}/TogglePublish")]
        public async Task<IActionResult> TogglePublish([FromRoute] int id , CancellationToken cancellationToken)
        {
            var state = await _pollService.TogglePublishedStateAsync(id, cancellationToken);
            return state is false ? NotFound() : NoContent();
        }
    }
}
