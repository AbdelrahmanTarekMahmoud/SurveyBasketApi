namespace SurveyBasket.Api.Contracts.Votes
{
    public class VoteRequestValidator  : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(x => x.Answers).NotEmpty();

            //to apply the child validator (voteAnswerRequest)
            RuleForEach(x => x.Answers).SetInheritanceValidator(x => x.Add(new VoteAnswerRequestValidator()));
        }
    }
}
