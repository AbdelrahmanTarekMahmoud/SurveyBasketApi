namespace SurveyBasket.Api.Contracts.Users
{
    //including roles
    public record UserResponse
    (
        string Id,
        string FirstName,
        string LastName,
        string Email,
        bool IsDisabled,
        IEnumerable<string> Roles
    );
}
