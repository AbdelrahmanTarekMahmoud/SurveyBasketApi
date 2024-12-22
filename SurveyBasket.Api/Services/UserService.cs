

namespace SurveyBasket.Api.Services
{
    public class UserService(ApplicationDbContext context ,
        UserManager<ApplicationUser> userManager
        , IRoleService roleService) : IUserService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IRoleService _roleService = roleService;

        public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string UserId)
        {
            var user = await _context.Users.Where(x => x.Id == UserId).ProjectToType<UserProfileResponse>().SingleAsync();


            return Result.Success<UserProfileResponse>(user);
        }





        public async Task<Result> UpdateUserProfileAsync(string userId, UserProfileUpdateRequest request, CancellationToken cancellationToken = default)
        {
            //var user = await _context.Users.Where(x => x.Id == userId).SingleAsync();

            //user.FirstName = request.FirstName;
            //user.LastName = request.LastName;

            //await _context.SaveChangesAsync();

            await _context.Users.Where(x => x.Id == userId)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.FirstName, request.FirstName)
                .SetProperty(x => x.LastName, request.LastName)
                );
            return Result.Success();  
        }



        public async Task<Result> ChangePasswordAsync(string UserId, UserChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.Where(x => x.Id == UserId).SingleAsync();
            var result =  await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if(result.Succeeded) 
            return Result.Success();

            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error.Code , error.Description , StatusCodes.Status400BadRequest));
        }



        //25 UserManagement
        public async Task<IEnumerable<UserResponse>> GetAllUsersDetailsAsync(CancellationToken cancellationToken = default)
        {
            //here we are grouping by Users 
            //because we starts from _contex.Users
            //so we have like this "Ahmed Khaled" : ["Admin" , "PollManager" , "RoleManager"] for example
           return await (
                   from u in _context.Users
                   join ur in _context.UserRoles
                   on u.Id equals ur.UserId
                   join r in _context.Roles
                   on ur.RoleId equals r.Id into roles
                   where !roles.Any(x => x.Name == DefaultRoles.Member)
                   select new
                       {
                         u.Id,
                         u.FirstName,
                         u.LastName,
                         u.Email,
                         u.IsDisabled,
                         Roles = roles.Select(x => x.Name).ToList()
                        }
                   )
                   .GroupBy(u => new {u.Id, u.FirstName, u.LastName , u.Email , u.IsDisabled})
                   .Select(u => new UserResponse(u.Key.Id , u.Key.FirstName , u.Key.LastName , u.Key.Email , u.Key.IsDisabled ,u.SelectMany(x => x.Roles)))
                   .ToListAsync(cancellationToken);

        }

        public async Task<Result<UserResponse>> GetUserDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            var User = await _userManager.FindByIdAsync(id);
            if(User is null)
            {
                return Result.Failure<UserResponse>(UserErrors.UserDoesnotExist);
            }
            
            var UserRoles = await _userManager.GetRolesAsync(User);

            //multiple Sources 
            var response = (User , UserRoles).Adapt<UserResponse>();
            //or this
            //var response = new UserResponse(id , User.FirstName , User.LastName , User.Email , User.IsDisabled
            //    , UserRoles);
            
            
            return Result.Success(response);
        }

        public async Task<Result<UserResponse>> CreateNewUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var IsThereExistingUserWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if(IsThereExistingUserWithSameEmail is not null)
            {
                return Result.Failure<UserResponse>(UserErrors.AlreadyRegisteredEmail);
            }

            var AllowedRoles = await _roleService.GetActiveRolesAsync(cancellationToken);
            if(request.Roles.Except(AllowedRoles.Select(x=>x.Name)).Any())
            {
                return Result.Failure<UserResponse>(RoleErrors.PermissionsInvalid);
            }

            var newUser = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(newUser , request.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRolesAsync(newUser, request.Roles);
                var response = (newUser , request.Roles).Adapt<UserResponse>();
                return Result.Success<UserResponse>(response);
            }
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> UpdateUserAsync(UpdateUserRequest request, string id, CancellationToken cancellationToken = default)
        {
            //check if there is any other user with same Email but diff id
            var isInvalidUser = await _context.Users.AnyAsync(x => x.Email == request.Email && x.Id != id);
            if(isInvalidUser)
            {
                return Result.Failure(UserErrors.InvalidCredentials);
            }

            var AllowedRoles = await _roleService.GetActiveRolesAsync(cancellationToken);
            if (request.Roles.Except(AllowedRoles.Select(x => x.Name)).Any())
            {
                return Result.Failure(RoleErrors.PermissionsInvalid);
            }

            //check if user with given id is exist
            var TargetUser = await _userManager.FindByIdAsync(id);
            if(TargetUser is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            //Fill the TargetUser with request info
            TargetUser = request.Adapt(TargetUser);
            TargetUser.UserName = request.Email;
            TargetUser.NormalizedUserName = request.Email.ToUpper();

            var result = await _userManager.UpdateAsync(TargetUser);

            if(result.Succeeded)
            {
                await _context.UserRoles.Where(x => x.UserId == id).ExecuteDeleteAsync(cancellationToken);
                await _userManager.AddToRolesAsync(TargetUser , request.Roles);
                return Result.Success();
            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error.Code , error.Description , StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ToggleUserStatusAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(id);

            if(user is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            user.IsDisabled = !user.IsDisabled;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }

        public async Task<Result> UnLockUser(string id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            await _userManager.SetLockoutEndDateAsync(user , null);
            

            return Result.Success();
        }
    }
}
