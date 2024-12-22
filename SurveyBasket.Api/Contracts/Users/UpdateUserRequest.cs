namespace SurveyBasket.Api.Contracts.Users
{
    public record UpdateUserRequest
    (
        string FirstName,
        string LastName,
        string Email,
        IList<string> Roles // to adapt to user response
    );
}
