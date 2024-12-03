
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Contracts.Polls;
using System.Xml.Linq;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // body json default
    [Authorize]
    public class PollsController (IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet]
        
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id , CancellationToken cancellationToken)
        {
            var element = await _pollService.GetAsync(id , cancellationToken);
            return element.IsFailure ?
                Problem(statusCode : StatusCodes.Status404NotFound , title : element.Error.Code , detail : element.Error.Description)
                : Ok(element.Value);
        }
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest poll , CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(poll , cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = newPoll.Value.Id }, newPoll.Value);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id , [FromBody] PollRequest poll , CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id , poll , cancellationToken);
            if (isUpdated.IsFailure) return Problem(statusCode: StatusCodes.Status404NotFound, title: isUpdated.Error.Code, detail: isUpdated.Error.Description);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id , CancellationToken cancellationToken)
        {
            var isDeleted = await _pollService.DeleteAsync(id , cancellationToken);
            if(isDeleted.IsFailure) return Problem(statusCode : StatusCodes.Status404NotFound , title:isDeleted.Error.Code , detail : isDeleted.Error.Description);
            return NoContent();
        }
        [HttpPut("{id}/TogglePublish")]
        public async Task<IActionResult> TogglePublish([FromRoute] int id , CancellationToken cancellationToken)
        {
            var state = await _pollService.TogglePublishedStateAsync(id, cancellationToken);
            return state.IsFailure ? Problem(statusCode: StatusCodes.Status404NotFound, title: state.Error.Code, detail: state.Error.Description) : NoContent();
        }
    }
}
