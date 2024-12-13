namespace SurveyBasket.Api.Contracts.Users
{
    public record UserProfileResponse
    (
        string FirstName,
        string LastName,
        string UserName,
        string Email
    );
}
