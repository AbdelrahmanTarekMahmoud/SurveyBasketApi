
namespace SurveyBasket.Api.Services
{
    public interface IResultService
    {
        public Task<Result<PollVotesResponse>> GetVoteResultsAsync(int pollId , CancellationToken cancellationToken);
        public Task<Result<IEnumerable<VotesInDayResponse>>> GetVoteInDaysAsync(int pollId, CancellationToken cancellationToken);

    }
}
