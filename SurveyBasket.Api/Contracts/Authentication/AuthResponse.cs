//include information that the client will need after a successful authentication
namespace SurveyBasket.Api.Contracts.Authentication
{
    public record AuthResponse
    (
        string Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        int ExpiresIn
    );
    
}
