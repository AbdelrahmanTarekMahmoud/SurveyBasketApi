
namespace SurveyBasket.Api.Contracts.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please Add title").Length(5, 100);
            RuleFor(x =>x.Description).NotEmpty().Length(5, 1000);

        }
    }
}
