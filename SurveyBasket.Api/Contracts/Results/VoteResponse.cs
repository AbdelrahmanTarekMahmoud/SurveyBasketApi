namespace SurveyBasket.Api.Contracts.Results
{
    //2
    //will be used by PollVoteResponse 
    public record VoteResponse
    (
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswers
        );

}
