

using SurveyBasket.Api.Errors;

namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider ) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;


        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
        {
            //Check user existance
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) 
            { 
               return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }
            //Correct Password
            var isValidPasssword = await _userManager.CheckPasswordAsync(user, Password);
            if (!isValidPasssword) 
            { 
                return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);  
            }

            //Generate Token
            var (token, expireIn) = _jwtProvider.GenerateToken(user);
            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn);
            return Result.Success<AuthResponse>(response);
            
        }
    }
}