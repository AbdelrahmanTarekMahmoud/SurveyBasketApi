

using SurveyBasket.Api.Contracts.Votes;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService questionService , IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteService = voteService;

        //return the questions and answers(all choices) to client
        [HttpGet("")]
        public async Task<IActionResult> BeginVote([FromRoute] int pollId , CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _questionService.GetAvailableAsync(pollId, userId! , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            
            var result = await _voteService.AddAsync(pollId, User.GetUserId(), voteRequest, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }

    }
}
