using SurveyBasket.Api.Contracts.Users;

namespace SurveyBasket.Api.Services
{
    public interface IUserService 
    {
        Task<Result<UserProfileResponse>> GetUserProfileAsync(string UserId);
        Task<Result> UpdateUserProfileAsync(string userId , UserProfileUpdateRequest request , CancellationToken cancellationToken = default);

        Task<Result> ChangePasswordAsync(string UserId, UserChangePasswordRequest request , CancellationToken cancellationToken = default);
    }
}
