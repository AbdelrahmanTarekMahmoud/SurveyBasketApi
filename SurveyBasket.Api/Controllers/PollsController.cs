
using Asp.Versioning;
using SurveyBasket.Api.Authentication.Filters;
using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Controllers
{
    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)]
    [Route("api/[controller]")]
    [ApiController] // body json default
    //[Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet("")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls);
        }

        [Authorize(Roles = DefaultRoles.Member)]
        [HttpGet("current")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetCurrentV1(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetCurrentAsyncV1(cancellationToken);
            return Ok(polls);
        }

        [Authorize(Roles = DefaultRoles.Member)]
        [HttpGet("current")]
        [MapToApiVersion(2)]
        public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetCurrentAsyncV2(cancellationToken);
            return Ok(polls);
        }


        [HttpGet("{id}")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.GetAsync(id, cancellationToken);
            return result.IsFailure ?
                result.ToProblem()
                : Ok(result.Value);
        }



        [HttpPost("")]
        [HasPermission(Permissions.AddPolls)]
        public async Task<IActionResult> Add([FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var result = await _pollService.AddAsync(poll, cancellationToken);
            //exception of duplicated titles
            if (result.IsFailure)
            {
                return result.ToProblem();
            }
            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }



        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, poll, cancellationToken);
            if (result.IsFailure) { return result.ToProblem(); }
            return NoContent();
        }



        [HttpDelete("{id}")]
        [HasPermission(Permissions.DeletePolls)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);
            if (result.IsFailure) return result.ToProblem();
            return NoContent();
        }



        [HttpPut("{id}/toggle-publish")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.TogglePublishedStateAsync(id, cancellationToken);
            return result.IsFailure ? result.ToProblem() : NoContent();
        }
    }
}
