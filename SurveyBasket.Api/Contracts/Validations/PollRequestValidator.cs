
namespace SurveyBasket.Api.Contracts.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please Add title").Length(5, 100);
            RuleFor(x =>x.Summary).NotEmpty().Length(5, 1000);
            RuleFor(x => x.StartsAt).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)); ;
            RuleFor(x => x.EndsAt).NotEmpty();
            RuleFor(x => x).Must(HasValidDate).WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} must be greater than or equal Start date");
            
        }
        public bool HasValidDate(PollRequest request)
        {
            return request.EndsAt >= request.StartsAt;
        }
    }
}
