namespace SurveyBasket.Api.Contracts.Results
{
    public record VotesInDayResponse
    (
        DateOnly Date,
        int NumberOfVotes
        ); 
}
