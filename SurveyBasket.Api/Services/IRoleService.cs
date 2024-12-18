using SurveyBasket.Api.Contracts.Roles;

namespace SurveyBasket.Api.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<RoleResponse>> GetActiveRolesAsync(CancellationToken cancellationToken = default);
        Task<Result<RoleDetailsResponse>> GetRoleByIdAsync(string id , CancellationToken cancellationToken = default);
        Task<Result<RoleDetailsResponse>> AddRoleAsync(RolesRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateRoleAsync(string id, RolesRequest request, CancellationToken cancellationToken = default);

        Task<Result> ToggleIsDeletedStatusAsync(string id , CancellationToken cancellationToken= default);
    }
}
