
namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider ) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;


        public async Task<AuthResponse?> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
        {
            //Check user existance
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) { return null; }
            //Correct Password
            var isValidPasssword = await _userManager.CheckPasswordAsync(user, Password);
            if (!isValidPasssword) { return null; }

            //Generate Token
            var (token, expireIn) = _jwtProvider.GenerateToken(user);
            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn);
            
        }
    }
}