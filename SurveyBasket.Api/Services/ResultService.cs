
namespace SurveyBasket.Api.Services
{
    public class ResultService(ApplicationDbContext context) : IResultService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVotesResponse>> GetVoteResultsAsync(int pollId , CancellationToken cancellationToken)
        {
            var PollResult = await _context.polls
                .Where(x => x.Id == pollId)
                .Select(x => new PollVotesResponse(
                    x.Title,
                    x.Votes.Select(x => new VoteResponse($"{x.User.FirstName} {x.User.LastName}", x.SumbittedOn
                                  , x.VoteAnswers.Select(x => new QuestionAnswerResponse(x.Question.Content, x.Answer.Content)))
                    ))).SingleOrDefaultAsync(cancellationToken);
            return PollResult is null ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound) : Result.Success<PollVotesResponse>(PollResult);

        }
        public async Task<Result<IEnumerable<VotesInDayResponse>>> GetVoteInDaysAsync(int pollId, CancellationToken cancellationToken)
        {
            var isPollExist = await _context.polls.AnyAsync(x => x.Id == pollId , cancellationToken);
            if (!isPollExist) 
            {
                return Result.Failure<IEnumerable<VotesInDayResponse>>(PollErrors.PollNotFound);
            }

            var votesPerDay = await _context.Votes.Where(x => x.PollId == pollId).
                GroupBy(x => new {Date = DateOnly.FromDateTime(x.SumbittedOn)}).
                Select(x => new VotesInDayResponse(x.Key.Date , x.Count())).ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotesInDayResponse>>(votesPerDay);    
        }
    }
}
