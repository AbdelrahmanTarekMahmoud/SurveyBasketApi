using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Api.Contracts.Votes;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Member)]
    [EnableRateLimiting("ConcurrencyLimiter")]
    public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteService = voteService;

        //return the questions and answers(all choices) to client
        [HttpGet("")]
        public async Task<IActionResult> BeginVote([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var userId = "68b4bb8d-fb62-48fd-8778-260225054de5";//User.GetUserId();
            var result = await _questionService.GetAvailableAsync(pollId, userId!, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        //Client answers question (questionid with the chosen answers (answerId))
        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest voteRequest, CancellationToken cancellationToken)
        {

            var result = await _voteService.AddAsync(pollId, User.GetUserId(), voteRequest, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }

    }
}
