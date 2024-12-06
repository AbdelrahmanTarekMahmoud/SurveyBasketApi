namespace SurveyBasket.Api.Errors
{
    public class VoteErrors
    {
        public static readonly Error DuplicatedVote =
         new("Vote.DuplicatedVote", "User already Voted for this Poll" , StatusCodes.Status409Conflict);

        public static readonly Error InvalidQuestions =
         new("Vote.InvalidQuestions", "Questions in poll in DB doenst match the Question in Request" , StatusCodes.Status400BadRequest);
    }
}
