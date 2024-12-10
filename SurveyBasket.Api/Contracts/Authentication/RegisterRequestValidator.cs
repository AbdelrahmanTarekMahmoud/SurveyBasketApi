using SurveyBasket.Api.Abstractions.Constants;

namespace SurveyBasket.Api.Contracts.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches(RegexPatterns.Password).WithMessage("Password is weak");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);

        }
    }
}
