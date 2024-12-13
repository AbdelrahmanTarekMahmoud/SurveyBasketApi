

namespace SurveyBasket.Api.Services
{
    public class UserService(ApplicationDbContext context ,
        UserManager<ApplicationUser> userManager) : IUserService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

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

    }
}
