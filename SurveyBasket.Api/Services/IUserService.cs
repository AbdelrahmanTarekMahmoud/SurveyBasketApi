namespace SurveyBasket.Api.Services
{
    public interface IUserService
    {
        Task<Result<UserProfileResponse>> GetUserProfileAsync(string UserId);
        Task<Result> UpdateUserProfileAsync(string userId, UserProfileUpdateRequest request, CancellationToken cancellationToken = default);

        Task<Result> ChangePasswordAsync(string UserId, UserChangePasswordRequest request, CancellationToken cancellationToken = default);

        //25 UserManagment
        Task<IEnumerable<UserResponse>> GetAllUsersDetailsAsync(CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetUserDetailsAsync(string id, CancellationToken cancellationToken = default);

        Task<Result<UserResponse>> CreateNewUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserAsync(UpdateUserRequest request, string id, CancellationToken cancellationToken = default);
        Task<Result> ToggleUserStatusAsync(string id, CancellationToken cancellationToken = default);
        Task<Result> UnLockUser(string id, CancellationToken cancellationToken = default);
    }
}
