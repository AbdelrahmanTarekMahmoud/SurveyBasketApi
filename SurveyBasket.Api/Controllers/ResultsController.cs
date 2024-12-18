using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Authentication.Filters;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [HasPermission(Permissions.Results)]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("voteResult")]
        public async Task<IActionResult> GetAllVotesResultInPoll([FromRoute] int pollId , CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVoteResultsAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("votePerDay")]
        public async Task<IActionResult> GetVotesInDays([FromRoute] int pollId , CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVoteInDaysAsync(pollId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }

}
