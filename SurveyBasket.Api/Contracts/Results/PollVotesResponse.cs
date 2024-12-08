namespace SurveyBasket.Api.Contracts.Results
{
    //1
    //return the result of votes of each poll
    public record PollVotesResponse
    (
        string Title,
        IEnumerable<VoteResponse> Votes
        );
}
