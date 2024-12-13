//responsible for generating token after validate login info


namespace SurveyBasket.Api.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default);

        Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);

        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);

        Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequest resendEmailConfirmationRequest);

        Task<Result> SendForgetPasswordCode(ForgetPasswordRequest request, CancellationToken cancellationToken = default);

        Task<Result> ResetPasswordForForgettingPassword(ResetPasswordRequest reset, CancellationToken cancellationToken = default);
    }
}
