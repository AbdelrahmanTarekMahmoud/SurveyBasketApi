
namespace SurveyBasket.Api.Contracts.Questions
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().Length(3, 1000);
            RuleFor(x => x.Answers).NotNull();

            RuleFor(x => x.Answers).Must(x => x.Count > 1).WithMessage("Each Question should has at least 2 answers")
                .When(x => x.Answers != null);
            RuleFor(x => x.Answers).Must(x => x.Distinct().Count() == x.Count).
                WithMessage("Same Question cannot has the same answer duplicated")
                .When(x => x.Answers != null);

        }

    }
}
