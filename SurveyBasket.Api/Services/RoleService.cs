using SurveyBasket.Api.Abstractions.Constants;
using SurveyBasket.Api.Contracts.Roles;
using System.Data;

namespace SurveyBasket.Api.Services
{
    public class RoleService(ApplicationDbContext context 
        ,RoleManager<ApplicationRole> roleManager) : IRoleService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles
                .Where(x => !x.IsDefault)
                .AsNoTracking()
                .Select(x => x.Adapt<RoleResponse>())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RoleResponse>> GetActiveRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles
                .Where(x => !x.IsDefault && !x.IsDeleted)
                .AsNoTracking()
                .Select(x => x.Adapt<RoleResponse>())
                .ToListAsync(cancellationToken);    
        }

        public async Task<Result<RoleDetailsResponse>> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var Role = await _roleManager.FindByIdAsync(id);
            if(Role == null)
            {
                return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);
            }
            
            var Permission = await _roleManager.GetClaimsAsync(Role);

            var Response = new RoleDetailsResponse(Role.Id, Role.Name, Role.IsDeleted
                , Permission.Select(x => x.Value)); //Cus Permission Here is List<claims> not List<string>

            return Result.Success(Response);

        }

        public async Task<Result<RoleDetailsResponse>> AddRoleAsync(RolesRequest request, CancellationToken cancellationToken = default)
        {
            var IsRoleWithSameNameAlreadyExist = await _roleManager.FindByNameAsync(request.Name);
            if(IsRoleWithSameNameAlreadyExist != null)
            {
                return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleDuplicated);
            }

            //make sure that request doesnt have a permissions that doenst exist in our permissions class
            var ExistedPermissions = Permissions.GetAllPermissions();
            
            if(request.Permissions.Except(ExistedPermissions).Any())
            {
                return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleDuplicated);
            }

            //Creating new ApplictionRole To add to Db
            var Role = new ApplicationRole
            {
                Name = request.Name,
                NormalizedName = request.Name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(Role);
            if(result.Succeeded)
            {
                //converting permissions of request "string" to IdentityRoleClaim
                var permissions = request.Permissions.Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = Role.Id,
                });
                await _context.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();

                //Constructing the Response
                var Response = new RoleDetailsResponse(Role.Id, Role.Name, Role.IsDeleted
                ,request.Permissions);
                
                return Result.Success(Response);
            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure<RoleDetailsResponse>(new Error ( error.Code ,  error.Description  , StatusCodes.Status400BadRequest));

        }

        public async Task<Result> UpdateRoleAsync(string id , RolesRequest request, CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is  null)
            {
                return Result.Failure(RoleErrors.RoleNotFound);
            }
            //Check if there is a role with the request name but different Id
            var IsRoleWithSameNameAlreadyExist = await _roleManager.Roles.AnyAsync(x => x.Name == request.Name
            && x.Id != id);

            if(IsRoleWithSameNameAlreadyExist)
            {
                return Result.Failure(RoleErrors.RoleDuplicated);
            }

            //make sure that request doesnt have a permissions that doenst exist in our permissions class
            var ExistedPermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(ExistedPermissions).Any())
            {
                return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleDuplicated);
            }

            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                // we used select to get only the value of claim
                var CurrentPermissions = await _context.RoleClaims
                    .Where(x => x.RoleId == id )
                    .Select(x => x.ClaimValue).ToListAsync();

                // those are the new permissions that need to be added 
                //but thier type is Ienumerable<string> and we need to insert them into the database
                //so we need to convert them first to IdentityRoleClaim
                var NewPermissions = request.Permissions
                    .Except(CurrentPermissions)
                    .Select(x => new IdentityRoleClaim<string>
                    {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = id
                    });

                //now we know the new permission "that doenst exist in db already"
                //now we need to know the opposite "that exist in db but not in our newPermissions"
                var NotNeededPermissions = CurrentPermissions.Except(request.Permissions);

                await _context.RoleClaims.Where(x => x.RoleId == id && NotNeededPermissions.Contains(x.ClaimValue))
                .ExecuteDeleteAsync();

                await _context.AddRangeAsync(NewPermissions);
                await _context.SaveChangesAsync();

                return Result.Success();
            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ToggleIsDeletedStatusAsync(string id, CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            if(role is null)
            {
                return Result.Failure(RoleErrors.RoleNotFound);
            }

            role.IsDeleted = !role.IsDeleted;
            await _context.SaveChangesAsync();
            return Result.Success();

        }
    }
}
