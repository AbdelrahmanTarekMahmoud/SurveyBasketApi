using SurveyBasket.Api.Authentication.Filters;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [HasPermission(Permissions.Results)]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("vote-result")]
        public async Task<IActionResult> GetAllVotesResultInPoll([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVoteResultsAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("vote-per-day")]
        public async Task<IActionResult> GetVotesInDays([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVoteInDaysAsync(pollId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }

}
