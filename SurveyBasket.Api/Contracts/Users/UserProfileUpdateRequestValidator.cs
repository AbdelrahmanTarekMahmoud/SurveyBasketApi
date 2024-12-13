namespace SurveyBasket.Api.Contracts.Users
{
    public class UserProfileUpdateRequestValidator : AbstractValidator<UserProfileUpdateRequest>
    {
        public UserProfileUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.LastName).NotEmpty()
                .Length(3, 100);

        }
    }
}
